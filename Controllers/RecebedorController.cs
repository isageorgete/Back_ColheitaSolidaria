using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.DTOs;
using Back_ColheitaSolidaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back_ColheitaSolidaria.Services;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecebedorController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RecebedorController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ------------------ CREATE ------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RecebedorRegisterDto dto)
        {
            if (await _context.Recebedores.AnyAsync(r => r.Email == dto.Email))
                return BadRequest("Já existe um recebedor com este email.");

            var recebedor = _mapper.Map<Recebedor>(dto);
            recebedor.SenhaHash = PasswordHasher.HashPassword(dto.Senha);

            _context.Recebedores.Add(recebedor);
            await _context.SaveChangesAsync();

            var recebedorResponse = _mapper.Map<RecebedorResponseDto>(recebedor);

            return CreatedAtAction(nameof(GetById), new { id = recebedor.Id }, recebedorResponse);
        }

        // ------------------ READ ------------------
        [Authorize(Roles = "Recebedor, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecebedorResponseDto>>> GetAll()
        {
            var recebedores = await _context.Recebedores.ToListAsync();
            var recebedorDtos = _mapper.Map<List<RecebedorResponseDto>>(recebedores);
            return Ok(recebedorDtos);
        }

        [Authorize(Roles = "Recebedor, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RecebedorResponseDto>> GetById(int id)
        {
            var recebedor = await _context.Recebedores.FindAsync(id);
            if (recebedor == null)
                return NotFound("Recebedor não encontrado!");

            var recebedorDto = _mapper.Map<RecebedorResponseDto>(recebedor);
            return Ok(recebedorDto);
        }

        // ------------------ UPDATE ------------------
        [Authorize(Roles = "Recebedor, Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RecebedorUpdateDto dto)
        {
            var recebedor = await _context.Recebedores.FindAsync(id);
            if (recebedor == null)
                return NotFound("Recebedor não encontrado!");

            var email = User.Identity?.Name;
            if (recebedor.Email != email && !User.IsInRole("Admin"))
                return Forbid("Você não pode alterar outro usuário!");


            _mapper.Map(dto, recebedor);

            if (!string.IsNullOrEmpty(dto.Senha))
                recebedor.SenhaHash = PasswordHasher.HashPassword(dto.Senha);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ------------------ DELETE ------------------
        [Authorize(Roles = "Recebedor, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recebedor = await _context.Recebedores.FindAsync(id);
            if (recebedor == null)
                return NotFound("Recebedor não encontrado!");


            var email = User.Identity?.Name;
            if (recebedor.Email != email && !User.IsInRole("Admin"))
                return Forbid("Você não pode alterar outro usuário!");



            _context.Recebedores.Remove(recebedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Recebedor, Admin")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var email = User.Identity?.Name;
            var recebedor = await _context.Recebedores.FirstOrDefaultAsync(r => r.Email == email);

            if (recebedor == null)
                return NotFound("Usuário não encontrado!");

            var recebedorDto = _mapper.Map<RecebedorResponseDto>(recebedor);
            return Ok(recebedorDto);
        }

        [Authorize(Roles = "Recebedor, Admin")]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] RecebedorUpdateDto dto)
        {
            var email = User.Identity?.Name;

            var recebedor = await _context.Recebedores.FirstOrDefaultAsync(r => r.Email == email);

            if (recebedor == null)
                return NotFound("Usuário não encontrado!");

            _mapper.Map(dto, recebedor);

            if (!string.IsNullOrEmpty(dto.Senha))
                recebedor.SenhaHash = PasswordHasher.HashPassword(dto.Senha);

            await _context.SaveChangesAsync();
            return Ok("Perfil atualizado com sucesso!");
        }



    }
}
