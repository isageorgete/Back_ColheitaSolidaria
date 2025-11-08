namespace Back_ColheitaSolidaria.DTOs.Solicitacoes
{
    public class SolicitacaoCreateDto
    {
        public int DoacaoId { get; set; }
        public int RecebedorId { get; set; } // ID do recebedor
        public int Quantidade { get; set; } // quantidade solicitada
    }
}
