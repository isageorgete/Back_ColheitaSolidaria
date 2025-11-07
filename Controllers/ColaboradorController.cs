using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.Services;
using Back_ColheitaSolidaria.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColaboradorController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ColaboradorController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ------------------ CREATE ------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ColaboradorRegisterDto dto)
        {
            if (dto.Senha != dto.ConfirmarSenha)
                return BadRequest("Senhas não conferem!");

            if (await _context.Colaboradores.AnyAsync(c => c.Email == dto.Email))
                return BadRequest("Já existe um Colaborador com este Email!");

            var colaborador = _mapper.Map<Colaborador>(dto);
            colaborador.SenhaHash = PasswordHasher.HashPassword(dto.Senha);

            _context.Colaboradores.Add(colaborador);
            await _context.SaveChangesAsync();

            var colaboradorResponse = _mapper.Map<ColaboradorResponseDto>(colaborador);

            return CreatedAtAction(nameof(GetById), new { id = colaborador.Id }, colaboradorResponse);
        }

        // ------------------ READ ------------------
        [Authorize(Roles = "Colaborador, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColaboradorResponseDto>>> GetAll()
        {
            var colaboradores = await _context.Colaboradores.ToListAsync();
            var colaboradorDtos = _mapper.Map<List<ColaboradorResponseDto>>(colaboradores);
            return Ok(colaboradorDtos);
        }

        [Authorize(Roles = "Colaborador, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ColaboradorResponseDto>> GetById(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);
            if (colaborador == null)
                return NotFound("Colaborador não encontrado!");

            var colaboradorDto = _mapper.Map<ColaboradorResponseDto>(colaborador);
            return Ok(colaboradorDto);
        }

        // ------------------ UPDATE ------------------
        [Authorize(Roles = "Colaborador, Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ColaboradorUpdateDto dto)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);
            if (colaborador == null)
                return NotFound("Colaborador não encontrado!");

            var email = User.Identity?.Name;

            if (colaborador.Email != email && !User.IsInRole("Admin"))
                return Forbid("Você não pode alterar outro usuário!");
           
            _mapper.Map(dto, colaborador);

            if (!string.IsNullOrEmpty(dto.Senha))
                colaborador.SenhaHash = PasswordHasher.HashPassword(dto.Senha);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ------------------ DELETE ------------------
        [Authorize(Roles = "Colaborador, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);
            if (colaborador == null)
                return NotFound("Colaborador não encontrado!");

            _context.Colaboradores.Remove(colaborador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Colaborador, Admin")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var email = User.Identity.Name;

            var colaborador = await _context.Colaboradores.FirstOrDefaultAsync(c => c.Email == email);

            if (colaborador == null)
                return NotFound("Usuário não encontrado!");

            var colaboradorDto = _mapper.Map<ColaboradorResponseDto>(colaborador);
            return Ok(colaboradorDto);
        }
        

        [Authorize(Roles = "Colaborador, Admin")]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] ColaboradorUpdateDto dto)
        {
            var email = User.Identity?.Name;

            var colaborador = await _context.Colaboradores.FirstOrDefaultAsync(c => c.Email == email);

            if (colaborador == null)
                return NotFound("Usuário não encontrado!");

            _mapper.Map(dto, colaborador);

            if (!string.IsNullOrEmpty(dto.Senha))
                colaborador.SenhaHash = PasswordHasher.HashPassword(dto.Senha);

            await _context.SaveChangesAsync();
            return Ok("Perfil atualizado com sucesso!");
        }


    }
}
