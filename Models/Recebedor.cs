using System.ComponentModel.DataAnnotations;

namespace Back_ColheitaSolidaria.Models
{
    public class Recebedor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 dígitos.")]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public int NumeroDeFamiliares { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        public string SenhaHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Recebedor";
        public string? FotoPerfil { get; set; }

    }
}
