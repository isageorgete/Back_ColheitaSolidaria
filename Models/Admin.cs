namespace Back_ColheitaSolidaria.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;

        // Para login
        // Testeeee
        public string ChaveAcesso { get; set; } = string.Empty;
    }
}
