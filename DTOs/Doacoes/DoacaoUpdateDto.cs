using Microsoft.AspNetCore.Http;

namespace Back_ColheitaSolidaria.DTOs.Doacoes
{
    public class DoacaoUpdateDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }

        // A imagem deve vir dentro do DTO, não como parâmetro separado
        public IFormFile? NovaImagem { get; set; }
    }
}