
public class AdminUpdateDto
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Endereco { get; set; } = string.Empty;

    public string? Senha { get; set; } = string.Empty;

}

