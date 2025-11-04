using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Back_ColheitaSolidaria.Services;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdminController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ------------------ CREATE (Cadastro público) ------------------
        [AllowAnonymous] // 🔹 Permite cadastro sem token
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminRegisterDto dto)
        {
            if (dto.Senha != dto.ConfirmarSenha)
                return BadRequest("Senhas não conferem!");

            if (await _context.Admins.AnyAsync(a => a.Email == dto.Email))
                return BadRequest("Já existe um Admin com este Email!");

            // Mapeia DTO → Entidade
            var admin = _mapper.Map<Admin>(dto);
            admin.SenhaHash = PasswordHasher.HashPassword(dto.Senha);

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            // Mapeia Entidade → DTO de resposta
            var adminResponse = _mapper.Map<AdminResponseDto>(admin);

            return CreatedAtAction(nameof(GetById), new { id = admin.Id }, adminResponse);
        }

        // ------------------ READ ------------------
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminResponseDto>>> GetAll()
        {
            var admins = await _context.Admins.ToListAsync();
            var adminDtos = _mapper.Map<List<AdminResponseDto>>(admins);
            return Ok(adminDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminResponseDto>> GetById(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
                return NotFound("Admin não encontrado!");

            var adminDto = _mapper.Map<AdminResponseDto>(admin);
            return Ok(adminDto);
        }

        // ------------------ UPDATE ------------------
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AdminUpdateDto dto)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
                return NotFound("Admin não encontrado!");

            _mapper.Map(dto, admin);
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
