using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.Services;
using Back_ColheitaSolidaria.DTOs;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColaboradorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ColaboradorController(AppDbContext context)
        {
            _context = context;
        }

        // ------------------ CREATE ------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ColaboradorRegisterDto dto)
        {
            if (dto.Senha != dto.ConfirmarSenha)
                return BadRequest("Senhas não conferem!");

            if (await _context.Colaboradores.AnyAsync(c => c.Email == dto.Email))
                return BadRequest("Já existe um Colaborador com este Email!");

            var colaborador = new Colaborador
            {
                NomeCompleto = dto.NomeCompleto,
                CPF = dto.CPF,
                DataNascimento = dto.DataNascimento,
                Email = dto.Email,
                Telefone = dto.Telefone,
                SenhaHash = PasswordHasher.HashPassword(dto.Senha)
            };

            _context.Colaboradores.Add(colaborador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = colaborador.Id }, new ColaboradorReadDto
            {
                Id = colaborador.Id,
                NomeCompleto = colaborador.NomeCompleto,
                CPF = colaborador.CPF,
                DataNascimento = colaborador.DataNascimento,
                Email = colaborador.Email,
                Telefone = colaborador.Telefone
            });
        }

        // ------------------ LOGIN ------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ColaboradorLoginDto dto)
        {
            var colaborador = await _context.Colaboradores.FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (colaborador == null)
                return Unauthorized("Colaborador não encontrado!");

            if (colaborador.SenhaHash != PasswordHasher.HashPassword(dto.Senha))
                return Unauthorized("Senha incorreta!");

            return Ok($"Login realizado com sucesso! Bem-vindo {colaborador.NomeCompleto}");
        }

        // ------------------ READ ------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColaboradorReadDto>>> GetAll()
        {
            var colaboradores = await _context.Colaboradores
                .Select(c => new ColaboradorReadDto
                {
                    Id = c.Id,
                    NomeCompleto = c.NomeCompleto,
                    CPF = c.CPF,
                    DataNascimento = c.DataNascimento,
                    Email = c.Email,
                    Telefone = c.Telefone
                })
                .ToListAsync();

            return Ok(colaboradores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColaboradorReadDto>> GetById(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);

            if (colaborador == null)
                return NotFound("Colaborador não encontrado!");

            return Ok(new ColaboradorReadDto
            {
                Id = colaborador.Id,
                NomeCompleto = colaborador.NomeCompleto,
                CPF = colaborador.CPF,
                DataNascimento = colaborador.DataNascimento,
                Email = colaborador.Email,
                Telefone = colaborador.Telefone
            });
        }

        // ------------------ UPDATE ------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ColaboradorUpdateDto dto)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);

            if (colaborador == null)
                return NotFound("Colaborador não encontrado!");

            colaborador.NomeCompleto = dto.NomeCompleto;
            colaborador.Telefone = dto.Telefone;

            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        // ------------------ DELETE ------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);

            if (colaborador == null)
                return NotFound("Colaborador não encontrado!");

            _context.Colaboradores.Remove(colaborador);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }
    }
}
