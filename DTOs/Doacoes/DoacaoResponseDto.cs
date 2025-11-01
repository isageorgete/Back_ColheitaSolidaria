namespace Back_ColheitaSolidaria.DTOs.Doacoes
{
    public class DoacaoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }
        public string ImagemUrl { get; set; }
        public int UsuarioId { get; set; }
    }
}
