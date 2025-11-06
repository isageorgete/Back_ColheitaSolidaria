using Microsoft.AspNetCore.Http;

namespace Back_ColheitaSolidaria.DTOs.Doacoes
{
    public class DoacaoCreateDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }

        // 🔹 URL pública da imagem (vinda do Supabase)
        public string? ImagemUrl { get; set; }
    }
}
