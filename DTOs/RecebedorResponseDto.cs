namespace Back_ColheitaSolidaria.DTOs
{
    public class RecebedorResponseDto
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public int NumeroDeFamiliares { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }
}
