namespace Back_ColheitaSolidaria.DTOs.Doacoes
{
    public class DoacaoUpdateDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }

        // Permite atualizar a imagem, se necessário
        public string? ImagemUrl { get; set; }
    }
}
