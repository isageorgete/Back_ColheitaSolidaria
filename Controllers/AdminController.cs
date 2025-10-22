using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.Services;
using Back_ColheitaSolidaria.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // ------------------ CREATE ------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminRegisterDto dto)
        {
            if (dto.Senha != dto.ConfirmarSenha)
                return BadRequest("Senhas não conferem!");

            if (await _context.Admins.AnyAsync(a => a.Email == dto.Email))
                return BadRequest("Já existe um Admin com este Email!");

            var admin = new Admin
            {
                NomeCompleto = dto.NomeCompleto,
                Cnpj = dto.Cnpj,
                DataNascimento = dto.DataNascimento,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Endereco = dto.Endereco,
                SenhaHash = PasswordHasher.HashPassword(dto.Senha),
                ChaveAcesso = dto.ChaveAcesso
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = admin.Id }, new AdminReadDto
            {
                Id = admin.Id,
                NomeCompleto = admin.NomeCompleto,
                Cnpj = admin.Cnpj,
                DataNascimento = admin.DataNascimento,
                Email = admin.Email,
                Telefone = admin.Telefone,
                Endereco = admin.Endereco,
                ChaveAcesso = admin.ChaveAcesso
            });
        }


        // ------------------ READ ------------------
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminReadDto>>> GetAll()
        {
            var admins = await _context.Admins
                .Select(a => new AdminReadDto
                {
                    Id = a.Id,
                    NomeCompleto = a.NomeCompleto,
                    Cnpj = a.Cnpj,
                    DataNascimento = a.DataNascimento,
                    Email = a.Email,
                    Telefone = a.Telefone,
                    Endereco = a.Endereco,
                    ChaveAcesso = a.ChaveAcesso
                })
                .ToListAsync();

            return Ok(admins);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminReadDto>> GetById(int id)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
                return NotFound("Admin não encontrado!");

            return Ok(new AdminReadDto
            {
                Id = admin.Id,
                NomeCompleto = admin.NomeCompleto,
                Cnpj = admin.Cnpj,
                DataNascimento = admin.DataNascimento,
                Email = admin.Email,
                Telefone = admin.Telefone,
                Endereco = admin.Endereco,
                ChaveAcesso = admin.ChaveAcesso
            });
        }

        // ------------------ UPDATE ------------------

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AdminUpdateDto dto)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
                return NotFound("Admin não encontrado!");

            admin.NomeCompleto = dto.NomeCompleto;
            admin.Cnpj = dto.Cnpj;
            admin.DataNascimento = dto.DataNascimento;
            admin.Email = dto.Email;
            admin.Telefone = dto.Telefone;
            admin.Endereco = dto.Endereco;

            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        // ------------------ DELETE ------------------
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
                return NotFound("Admin não encontrado!");

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }
    }
}
