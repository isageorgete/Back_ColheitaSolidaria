namespace Back_ColheitaSolidaria.DTOs
{
    public class UpdateRoleDto
    {
        public string TipoUsuario { get; set; } // "colaborador" ou "recebedor"
        public int IdUsuario { get; set; }
        public string NovoRole { get; set; } // "Admin", "Colaborador", "Recebedor"
    }
}
