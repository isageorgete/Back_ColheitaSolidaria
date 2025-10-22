using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.DTOs;
using Back_ColheitaSolidaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back_ColheitaSolidaria.Services;
using Microsoft.AspNetCore.Authorization;

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
            if (await _context.Recebedores.AnyAsync(r => r.Email == dto.Email))
                return BadRequest("Já existe um recebedor com este email.");

            var recebedor = new Recebedor
            {
                NomeCompleto = dto.NomeCompleto,
                Cpf = dto.Cpf,
                DataNascimento = dto.DataNascimento,
                NumeroDeFamiliares = dto.NumeroDeFamiliares,
                Email = dto.Email,
                Telefone = dto.Telefone,
                SenhaHash = PasswordHasher.HashPassword(dto.Senha) // uso estático
            };

            _context.Recebedores.Add(recebedor);
            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Recebedor registrado com sucesso", recebedor.Id });
        }


        [Authorize(Roles = "Recebedor")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecebedorReadDto>>> GetAll()
        {
            var recebedores = await _context.Recebedores
                .Select(r => new RecebedorReadDto
                {
                    Id = r.Id,
                    NomeCompleto = r.NomeCompleto,
                    Cpf = r.Cpf,
                    DataNascimento = r.DataNascimento,
                    NumeroDeFamiliares = r.NumeroDeFamiliares,
                    Email = r.Email,
                    Telefone = r.Telefone
                })
                .ToListAsync();

            return Ok(recebedores);
        }
        [Authorize(Roles = "Recebedor")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RecebedorReadDto>> GetById(int id)
        {
            var recebedor = await _context.Recebedores.FindAsync(id);
            if (recebedor == null) return NotFound();

            return Ok(new RecebedorReadDto
            {
                Id = recebedor.Id,
                NomeCompleto = recebedor.NomeCompleto,
                Cpf = recebedor.Cpf,
                DataNascimento = recebedor.DataNascimento,
                NumeroDeFamiliares = recebedor.NumeroDeFamiliares,
                Email = recebedor.Email,
                Telefone = recebedor.Telefone
            });
        }
        [Authorize(Roles = "Recebedor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RecebedorUpdateDto dto)
        {
            var recebedor = await _context.Recebedores.FindAsync(id);
            if (recebedor == null) return NotFound();

            recebedor.NomeCompleto = dto.NomeCompleto;
            recebedor.NumeroDeFamiliares = dto.NumeroDeFamiliares;
            recebedor.Telefone = dto.Telefone;

            if (!string.IsNullOrEmpty(dto.Senha))
                recebedor.SenhaHash = PasswordHasher.HashPassword(dto.Senha); // estático

            await _context.SaveChangesAsync();

            return Ok("Recebedor atualizado com sucesso.");
        }
        [Authorize(Roles = "Recebedor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recebedor = await _context.Recebedores.FindAsync(id);
            if (recebedor == null) return NotFound();

            _context.Recebedores.Remove(recebedor);
            await _context.SaveChangesAsync();

            return Ok("Recebedor deletado com sucesso.");
        }
    }
}
