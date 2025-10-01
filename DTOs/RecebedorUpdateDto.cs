namespace Back_ColheitaSolidaria.DTOs
{
    public class RecebedorUpdateDto
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public int NumeroDeFamiliares { get; set; }
        public string Telefone { get; set; } = string.Empty;

        public string? Senha { get; set; } = string.Empty;
    }
}
