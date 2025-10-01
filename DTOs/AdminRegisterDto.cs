namespace Back_ColheitaSolidaria.DTOs
{
    public class AdminRegisterDto
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Senha { get; set; } = string.Empty;
        public string ConfirmarSenha { get; set; } = string.Empty;
        public string ChaveAcesso { get; set; } = string.Empty;
    }
}
