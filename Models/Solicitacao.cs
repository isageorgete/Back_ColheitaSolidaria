namespace Back_ColheitaSolidaria.Models
{
    public class Solicitacao
    {
        public int Id { get; set; }
        public int DoacaoId { get; set; }
        public int RecebedorId { get; set; }       // aqui substitui UsuarioId
        public int QuantidadeSolicitada { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string Status { get; set; } = "pendente";

        // Relações
        public Doacao Doacao { get; set; }
        public Recebedor Recebedor { get; set; }
    }
}
