namespace Back_ColheitaSolidaria.DTOs.Doacoes
{
    public class DoacaoListDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }
        public string? ImagemUrl { get; set; }
        public int UsuarioId { get; set; }
        public string ColaboradorNome { get; set; } = "Desconhecido";
    }
}
