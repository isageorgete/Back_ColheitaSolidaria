using Back_ColheitaSolidaria.DTOs.Solicitacoes;
using Back_ColheitaSolidaria.Services.Solicitacoes;
using Microsoft.AspNetCore.Mvc;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitacaoController : ControllerBase
    {
        private readonly SolicitacaoService _service;

        public SolicitacaoController(SolicitacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SolicitacaoCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, SolicitacaoUpdateDto dto)
        {
            var result = await _service.UpdateStatusAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // Aprovar solicitação
        [HttpPut("{id}/aprovar")]
        public async Task<IActionResult> AprovarSolicitacao(int id)
        {
            var result = await _service.AprovarAsync(id);
            if (result == null) return NotFound(new { message = "Solicitação não encontrada." });

            return Ok(new
            {
                message = "Solicitação aprovada com sucesso!",
                solicitacao = result
            });
        }

        // Negar solicitação
        [HttpPut("{id}/negar")]
        public async Task<IActionResult> NegarSolicitacao(int id)
        {
            var result = await _service.NegarAsync(id);
            if (result == null) return NotFound(new { message = "Solicitação não encontrada." });

            return Ok(new
            {
                message = "Solicitação negada com sucesso!",
                solicitacao = result
            });
        }
    }
}
