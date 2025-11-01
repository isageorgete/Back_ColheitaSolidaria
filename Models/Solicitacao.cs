namespace Back_ColheitaSolidaria.Models
{
    public class Solicitacao
    {
        public int Id { get; set; }
        public DateTime DataSolicitacao { get; set; } = DateTime.Now;
        public int DoacaoId { get; set; }
        public string Status { get; set; } = "Pendente"; // Pendente, Aprovado, Negado
        public int UsuarioId { get; set; }
    }
}
