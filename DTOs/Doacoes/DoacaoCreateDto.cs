using Microsoft.AspNetCore.Http;

namespace Back_ColheitaSolidaria.DTOs.Doacoes
{
    public class DoacaoCreateDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }

        // O usuário só envia o arquivo da imagem
        public IFormFile Imagem { get; set; }

        // REMOVER esta propriedade - será gerada pelo backend
        // public string UrlImagem { get; set; }
    }
}