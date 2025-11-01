using System;

namespace Back_ColheitaSolidaria.Models
{
    public class Doacao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public DateTime Validade { get; set; }
        public string ImagemUrl { get; set; } // URL da imagem no Supabase
        public int UsuarioId { get; set; } // Quem cadastrou
    }
}
