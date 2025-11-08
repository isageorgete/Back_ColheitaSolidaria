using System.ComponentModel.DataAnnotations.Schema;

namespace Back_ColheitaSolidaria.Models
{
    public class Doacao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Colaborador? Usuario { get; set; }
    }
}
