using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Back_ColheitaSolidaria.Services;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto dto)
        {
            object usuario = null;
            string role = dto.TipoUsuario;

            // Buscar usuário pelo tipo
            switch (dto.TipoUsuario.ToLower())
            {
                case "admin":
                    usuario = await _context.Admins.FirstOrDefaultAsync(a => a.Email == dto.Email);
                    break;
                case "colaborador":
                    usuario = await _context.Colaboradores.FirstOrDefaultAsync(d => d.Email == dto.Email);
                    break;
                case "recebedor":
                    usuario = await _context.Recebedores.FirstOrDefaultAsync(r => r.Email == dto.Email);
                    break;
                default:
                    return BadRequest("Tipo de usuário inválido.");
            }

            if (usuario == null)
                return Unauthorized("Usuário não encontrado!");

            // Verificar senha (cada modelo tem SenhaHash)
            string senhaHash = dto.TipoUsuario.ToLower() switch
            {
                "admin" => ((Admin)usuario).SenhaHash,
                "colaborador" => ((Colaborador)usuario).SenhaHash,
                "recebedor" => ((Recebedor)usuario).SenhaHash,
                _ => ""
            };

            if (senhaHash != PasswordHasher.HashPassword(dto.Senha))
                return Unauthorized("Senha incorreta!");

            // Gerar token JWT
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
        new Claim(ClaimTypes.Name, dto.Email),
        new Claim(ClaimTypes.Role, role.First().ToString().ToUpper() + role.Substring(1).ToLower()) // "Admin", "Colaborador" ou "Recebedor"
    }),
                Expires = DateTime.Now.AddDays(7),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Message = $"Login realizado com sucesso! Bem-vindo {dto.Email}",
                Token = tokenString,
                UserId = usuario switch
                {
                    Admin a => a.Id,
                    Colaborador c => c.Id,
                    Recebedor r => r.Id,
                    _ => 0
                }
            });


        }
    }
}

