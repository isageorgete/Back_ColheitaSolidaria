using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Back_ColheitaSolidaria.DTOs.Doacoes;
using Back_ColheitaSolidaria.Services.Doacoes;

namespace Back_ColheitaSolidaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoacaoController : ControllerBase
    {
        private readonly DoacaoService _service;

        public DoacaoController(DoacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doacoes = await _service.GetAllAsync();
            return Ok(doacoes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Doação não encontrada." });

            return Ok(result);
        }

        // 🔒 Requer autenticação
        [Authorize]
        [HttpPost]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> Create([FromBody] DoacaoCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Os dados da doação são obrigatórios.");

                if (string.IsNullOrWhiteSpace(dto.ImagemUrl))
                    return BadRequest("A URL da imagem é obrigatória.");

                // 🧩 Pega o e-mail do usuário logado (vem do JWT)
                var userEmail = User.Identity?.Name;

                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized(new { message = "Usuário não autenticado." });

                var result = await _service.CreateAsync(dto, userEmail);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao criar doação: {ex.Message}");
                return StatusCode(500, new { message = "Erro interno ao criar a doação.", error = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DoacaoUpdateDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto);
                if (result == null)
                    return NotFound(new { message = "Doação não encontrada." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao atualizar doação: {ex.Message}");
                return StatusCode(500, new { message = "Erro interno ao atualizar a doação.", error = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = "Doação não encontrada." });

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao deletar doação: {ex.Message}");
                return StatusCode(500, new { message = "Erro interno ao deletar a doação.", error = ex.Message });
            }
        }

        // 🔹 Nova rota: buscar doações de um colaborador específico
        [Authorize]
        [HttpGet("Colaborador/{usuarioId}")]
        public async Task<IActionResult> GetByColaborador(int usuarioId)
        {
            try
            {
                var doacoes = await _service.GetByUsuarioIdAsync(usuarioId);

                if (doacoes == null || !doacoes.Any())
                    return NotFound(new { message = "Nenhuma doação encontrada para este colaborador." });

                return Ok(doacoes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar doações por colaborador: {ex.Message}");
                return StatusCode(500, new { message = "Erro interno ao buscar doações.", error = ex.Message });
            }
        }
    }
}
