namespace Back_ColheitaSolidaria.DTOs.Solicitacoes
{
    public class SolicitacaoResponseDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DataSolicitacao { get; set; }
        public string RecebedorNome { get; set; } = string.Empty;  // nome do usuário
        public string DoacaoNome { get; set; } = string.Empty;       // nome do alimento
        public string DoacaoDescricao { get; set; } = string.Empty;  // descrição
    }
}
