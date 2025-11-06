namespace Back_ColheitaSolidaria.DTOs.Doacoes
{
    public class DoacaoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;
    }
}
