namespace Back_ColheitaSolidaria.DTOs.Solicitacoes
{
    public class SolicitacaoResponseDto
    {
        public int Id { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public int DoacaoId { get; set; }
        public string Status { get; set; }
        public int UsuarioId { get; set; }
    }
}
