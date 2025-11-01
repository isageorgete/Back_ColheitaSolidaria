using Back_ColheitaSolidaria.DTOs.Doacoes;
using Back_ColheitaSolidaria.Services.Doacoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DoacaoCreateDto dto)
        {
            try
            {
                Console.WriteLine("=== INICIANDO CREATE DOAÇÃO ===");
                Console.WriteLine($"Nome: {dto.Nome}");
                Console.WriteLine($"Descricao: {dto.Descricao}");
                Console.WriteLine($"Quantidade: {dto.Quantidade}");
                Console.WriteLine($"Validade: {dto.Validade}");
                Console.WriteLine($"Imagem: {dto.Imagem?.FileName} ({dto.Imagem?.Length} bytes)");

                if (dto.Imagem != null)
                {
                    // Validações básicas
                    if (dto.Imagem.Length == 0)
                    {
                        Console.WriteLine("Arquivo vazio detectado");
                        return BadRequest("Arquivo de imagem vazio");
                    }

                    if (dto.Imagem.Length > 10 * 1024 * 1024) // 10MB
                    {
                        Console.WriteLine("Arquivo muito grande");
                        return BadRequest("Arquivo muito grande. Máximo 10MB.");
                    }
                }

                var result = await _service.CreateAsync(dto);
                Console.WriteLine("Doação criada com sucesso!");

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO NO CONTROLLER: {ex}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] DoacaoUpdateDto dto)
        {
            // Agora passa apenas 2 argumentos: id e dto
            var result = await _service.UpdateAsync(id, dto);
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
    }
}
