namespace Back_ColheitaSolidaria.DTOs
{
    public class AdminRegisterDto
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;

        public string ChaveAcesso { get; set; } = string.Empty; // usado no login
        public string Senha { get; set; } = string.Empty;        // senha em texto (vai virar hash)
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
