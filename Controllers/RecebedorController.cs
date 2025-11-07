using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.DTOs;
using Back_ColheitaSolidaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back_ColheitaSolidaria.DTOs.Solicitacoes;

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

        // 🔹 Solicitar um alimento (sem JWT, usando userId do frontend)
        [HttpPost("Solicitar")]
        public async Task<IActionResult> Solicitar([FromBody] SolicitarAlimentoDto dto)
        {
            if (dto.UserId <= 0)
                return BadRequest("Erro: usuário não identificado.");

            // Verifica se o recebedor existe
            var recebedor = await _context.Recebedores.FindAsync(dto.UserId);
            if (recebedor == null)
                return NotFound("Recebedor não encontrado.");

            // Verifica se a doação existe
            var doacao = await _context.Doacoes.FindAsync(dto.DoacaoId);
            if (doacao == null)
                return NotFound("Doação não encontrada.");

            // Aqui você pode criar a entidade de solicitação
            var solicitacao = new Solicitacao
            {
                RecebedorId = dto.UserId,
                DoacaoId = dto.DoacaoId,
                DataSolicitacao = DateTime.Now,
                QuantidadeSolicitada = dto.Quantidade
            };

            _context.Solicitacoes.Add(solicitacao);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Alimento solicitado com sucesso!", solicitacaoId = solicitacao.Id });
        }
    }
}
