namespace Back_ColheitaSolidaria.DTOs.Solicitacoes
{
    public class SolicitarAlimentoDto
    {
        public int DoacaoId { get; set; }
        public int UserId { get; set; }       // id do recebedor
        public int Quantidade { get; set; }   // quantidade solicitada
    }
}
