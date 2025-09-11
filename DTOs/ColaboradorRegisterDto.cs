namespace Back_ColheitaSolidaria.DTOs
{
    public class ColaboradorRegisterDto
    {
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public string ConfirmarSenha { get; set; }
    }
}
