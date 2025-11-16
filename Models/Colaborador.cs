namespace Back_ColheitaSolidaria.Models
{
    public class Colaborador
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Colaborador";
        public ICollection<Doacao>? Doacoes { get; set; }
        public string? FotoPerfil { get; set; }

    }
}
