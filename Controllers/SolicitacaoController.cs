using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.DTOs.Solicitacoes;
using Back_ColheitaSolidaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitacaoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SolicitacaoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ------------------ CREATE ------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SolicitacaoCreateDto dto)
        {
            if (dto.DoacaoId == 0)
                return BadRequest("Doação não informada.");

            if (dto.UsuarioId == 0)
                return BadRequest("Usuário não identificado.");

            var doacao = await _context.Doacoes.FindAsync(dto.DoacaoId);
            if (doacao == null)
                return NotFound("Doação não encontrada.");

            // Mapeia DTO para modelo
            var solicitacao = _mapper.Map<Solicitacao>(dto);
            solicitacao.DataSolicitacao = DateTime.Now;
            solicitacao.Status = "pendente";

            _context.Solicitacoes.Add(solicitacao);
            await _context.SaveChangesAsync();

            var responseDto = _mapper.Map<SolicitacaoResponseDto>(solicitacao);
            return CreatedAtAction(nameof(GetById), new { id = solicitacao.Id }, responseDto);
        }

        // ------------------ READ ------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var solicitacoes = await _context.Solicitacoes.ToListAsync();
            var response = _mapper.Map<List<SolicitacaoResponseDto>>(solicitacoes);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var solicitacao = await _context.Solicitacoes.FindAsync(id);
            if (solicitacao == null)
                return NotFound("Solicitação não encontrada.");

            var response = _mapper.Map<SolicitacaoResponseDto>(solicitacao);
            return Ok(response);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var solicitacoes = await _context.Solicitacoes
                .Where(s => s.RecebedorId == usuarioId)
                .ToListAsync();

            var response = _mapper.Map<List<SolicitacaoResponseDto>>(solicitacoes);
            return Ok(response);
        }

        // ------------------ UPDATE ------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SolicitacaoUpdateDto dto)
        {
            var solicitacao = await _context.Solicitacoes.FindAsync(id);
            if (solicitacao == null)
                return NotFound("Solicitação não encontrada.");

            if (string.IsNullOrEmpty(dto.Status) || (dto.Status != "Aprovado" && dto.Status != "Negado"))
                return BadRequest("Status inválido.");

            solicitacao.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok("Solicitação atualizada com sucesso.");
        }

        // ------------------ DELETE ------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var solicitacao = await _context.Solicitacoes.FindAsync(id);
            if (solicitacao == null)
                return NotFound("Solicitação não encontrada.");

            _context.Solicitacoes.Remove(solicitacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Solicitacao/usuario/2
        [HttpGet("usuario/{usuarioId}/historico")]
        public async Task<IActionResult> GetSolicitacoesPorUsuario(int usuarioId)
        {
            var solicitacoes = await _context.Solicitacoes
                .Include(s => s.Doacao)
                .Where(s => s.RecebedorId == usuarioId)
                .OrderByDescending(s => s.DataSolicitacao)
                .ToListAsync();

            if (!solicitacoes.Any())
                return NotFound(new { message = "Nenhuma solicitação encontrada para este usuário." });

            var resultado = solicitacoes.Select(s => new
            {
                s.Id,
                s.DataSolicitacao,
                s.Status,
                Doacao = new
                {
                    s.Doacao.Id,
                    s.Doacao.Nome,
                    s.Doacao.Descricao,
                    s.Doacao.ImagemUrl
                }
            });

            return Ok(resultado);
        }

    }
}
