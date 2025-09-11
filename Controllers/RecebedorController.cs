using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.DTOs;
using Back_ColheitaSolidaria.Services;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecebedorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecebedorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RecebedorRegisterDto dto)
        {
            if (dto.Senha != dto.ConfirmarSenha)
                return BadRequest("Senhas não conferem!");

            if (await _context.Recebedores.AnyAsync(r => r.Email == dto.Email))
                return BadRequest("Já existe um Recebedor com este Email!");

            var recebedor = new Recebedor
            {
                NomeCompleto = dto.NomeCompleto,
                Cpf = dto.CPF,
                DataNascimento = dto.DataNascimento,
                NumeroDeFamiliares = dto.NumeroDeFamiliares,
                Email = dto.Email,
                Telefone = dto.Telefone,
                SenhaHash = PasswordHasher.HashPassword(dto.Senha)
            };

            _context.Recebedores.Add(recebedor);
            await _context.SaveChangesAsync();

            return Ok("Recebedor cadastrado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RecebedorLoginDto dto)
        {
            var recebedor = await _context.Recebedores.FirstOrDefaultAsync(r => r.Email == dto.Email);

            if (recebedor == null)
                return Unauthorized("Recebedor não encontrado!");

            if (recebedor.SenhaHash != PasswordHasher.HashPassword(dto.Senha))
                return Unauthorized("Senha incorreta!");

            return Ok($"Login realizado com sucesso! Bem-vindo {recebedor.NomeCompleto}");
        }
    }
}

