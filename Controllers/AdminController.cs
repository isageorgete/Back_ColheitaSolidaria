using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.DTOs;
using Back_ColheitaSolidaria.Services;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // -> /api/admin
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminRegisterDto dto)
        {
            // Validações básicas
            if (dto.Senha != dto.ConfirmarSenha)
                return BadRequest("Senhas não conferem!");

            if (await _context.Admins.AnyAsync(a => a.Email == dto.Email))
                return BadRequest("Já existe um Admin com este Email!");

            if (await _context.Admins.AnyAsync(a => a.Cnpj == dto.Cnpj))
                return BadRequest("Já existe um Admin com este CNPJ!");

            // Monta o model e aplica hash na senha
            var admin = new Admin
            {
                NomeCompleto = dto.NomeCompleto,
                Cnpj = dto.Cnpj,
                DataNascimento = dto.DataNascimento,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Endereco = dto.Endereco,
                ChaveAcesso = dto.ChaveAcesso,
                SenhaHash = PasswordHasher.HashPassword(dto.Senha)
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return Ok("Admin cadastrado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginDto dto)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a =>
                a.Cnpj == dto.Cnpj &&
                a.Email == dto.Email &&
                a.ChaveAcesso == dto.ChaveAcesso);

            if (admin == null)
                return Unauthorized("Admin não encontrado!");

            if (admin.SenhaHash != PasswordHasher.HashPassword(dto.Senha))
                return Unauthorized("Senha incorreta!");

            return Ok($"Login realizado com sucesso! Bem-vindo {admin.NomeCompleto}");
        }
    }
}
