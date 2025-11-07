using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_ColheitaSolidaria.Migrations
{
    public partial class AddRoleToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Recebedores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Colaboradores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Recebedores");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Colaboradores");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Admins");
        }
    }
}
