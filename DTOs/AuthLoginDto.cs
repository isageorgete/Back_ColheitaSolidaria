namespace Back_ColheitaSolidaria.DTOs
{
    public class AuthLoginDto
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string TipoUsuario { get; set; } // "Admin", "Colaborador" ou "Recebedor"
    }
}
