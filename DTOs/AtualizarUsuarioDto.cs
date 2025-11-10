namespace Back_ColheitaSolidaria.DTOs
{
    public class AtualizarUsuarioDto
    {
        // Campos comuns
        public string? NomeCompleto { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Senha { get; set; }

        // Campos específicos
        public string? Cpf { get; set; }              // Colaborador / Recebedor
        public string? Cnpj { get; set; }             // Admin
        public string? Endereco { get; set; }         // Admin
        public int? NumeroDeFamiliares { get; set; }  // Recebedor
    }
}
