using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.DTOs;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Senha))
                return BadRequest("E-mail e senha são obrigatórios.");

            object usuario = null;
            string role = dto.TipoUsuario;

            switch (dto.TipoUsuario.ToLower())
            {
                case "admin":
                    usuario = await _context.Admins.FirstOrDefaultAsync(a => a.Email == dto.Email);
                    break;
                case "colaborador":
                    usuario = await _context.Colaboradores.FirstOrDefaultAsync(c => c.Email == dto.Email);
                    break;
                case "recebedor":
                    usuario = await _context.Recebedores.FirstOrDefaultAsync(r => r.Email == dto.Email);
                    break;
                default:
                    return BadRequest("Tipo de usuário inválido.");
            }

            if (usuario == null)
                return Unauthorized("Usuário não encontrado!");

            string senhaHash = dto.TipoUsuario.ToLower() switch
            {
                "admin" => ((Admin)usuario).SenhaHash,
                "colaborador" => ((Colaborador)usuario).SenhaHash,
                "recebedor" => ((Recebedor)usuario).SenhaHash,
                _ => ""
            };

            if (senhaHash != PasswordHasher.HashPassword(dto.Senha))
                return Unauthorized("Senha incorreta!");

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            int userId = usuario switch
            {
                Admin a => a.Id,
                Colaborador c => c.Id,
                Recebedor r => r.Id,
                _ => 0
            };

            string nomeUsuario = usuario switch
            {
                Admin a => a.NomeCompleto,
                Colaborador c => c.NomeCompleto,
                Recebedor r => r.NomeCompleto,
                _ => dto.Email
            };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, dto.Email),
                new Claim(ClaimTypes.Name, dto.Email), // ← garante que User.Identity.Name funcione
                new Claim(ClaimTypes.Role, char.ToUpper(role[0]) + role.Substring(1).ToLower(), ClaimValueTypes.String),
                new Claim("nome", nomeUsuario)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(4),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                message = $"Login realizado com sucesso! Bem-vindo {nomeUsuario}",
                token = tokenString,
                usuario = new
                {
                    id = userId,
                    nome = nomeUsuario,
                    email = dto.Email,
                    role = role
                }
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetUsuarioLogado()
        {
            try
            {
                var email = User.Identity?.Name;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (email == null || role == null)
                    return Unauthorized(new { message = "Token inválido ou ausente." });

                object usuario = role.ToLower() switch
                {
                    "admin" => await _context.Admins.FirstOrDefaultAsync(a => a.Email == email),
                    "colaborador" => await _context.Colaboradores.FirstOrDefaultAsync(c => c.Email == email),
                    "recebedor" => await _context.Recebedores.FirstOrDefaultAsync(r => r.Email == email),
                    _ => null
                };

                if (usuario == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao obter usuário logado.", error = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> AtualizarUsuarioLogado([FromBody] AtualizarUsuarioDto dto)
        {
            try
            {
                var email = User.Identity?.Name;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (email == null || role == null)
                    return Unauthorized(new { message = "Token inválido ou ausente." });

                object usuario = null;

                switch (role.ToLower())
                {
                    // 🔹 ADMIN
                    case "admin":
                        usuario = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
                        if (usuario is Admin admin)
                        {
                            admin.NomeCompleto = dto.NomeCompleto ?? admin.NomeCompleto;
                            admin.Cnpj = dto.Cnpj ?? admin.Cnpj;
                            admin.Telefone = dto.Telefone ?? admin.Telefone;
                            admin.Endereco = dto.Endereco ?? admin.Endereco;
                            admin.DataNascimento = dto.DataNascimento ?? admin.DataNascimento;

                            if (!string.IsNullOrWhiteSpace(dto.Senha))
                                admin.SenhaHash = PasswordHasher.HashPassword(dto.Senha);
                        }
                        break;

                    // 🔹 COLABORADOR
                    case "colaborador":
                        usuario = await _context.Colaboradores.FirstOrDefaultAsync(c => c.Email == email);
                        if (usuario is Colaborador colab)
                        {
                            colab.NomeCompleto = dto.NomeCompleto ?? colab.NomeCompleto;
                            colab.CPF = dto.Cpf ?? colab.CPF;
                            colab.Telefone = dto.Telefone ?? colab.Telefone;
                            colab.DataNascimento = dto.DataNascimento ?? colab.DataNascimento;

                            if (!string.IsNullOrWhiteSpace(dto.Senha))
                                colab.SenhaHash = PasswordHasher.HashPassword(dto.Senha);
                        }
                        break;

                    // 🔹 RECEBEDOR
                    case "recebedor":
                        usuario = await _context.Recebedores.FirstOrDefaultAsync(r => r.Email == email);
                        if (usuario is Recebedor rec)
                        {
                            rec.NomeCompleto = dto.NomeCompleto ?? rec.NomeCompleto;
                            rec.Cpf = dto.Cpf ?? rec.Cpf;
                            rec.Telefone = dto.Telefone ?? rec.Telefone;
                            rec.DataNascimento = dto.DataNascimento ?? rec.DataNascimento;
                            rec.NumeroDeFamiliares = dto.NumeroDeFamiliares ?? rec.NumeroDeFamiliares;

                            if (!string.IsNullOrWhiteSpace(dto.Senha))
                                rec.SenhaHash = PasswordHasher.HashPassword(dto.Senha);
                        }
                        break;

                    default:
                        return BadRequest(new { message = "Tipo de usuário inválido." });
                }

                if (usuario == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                await _context.SaveChangesAsync();

                return Ok(new { message = "Dados atualizados com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao atualizar dados.", error = ex.Message });
            }
        }


    }
}
