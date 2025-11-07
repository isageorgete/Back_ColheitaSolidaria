using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_ColheitaSolidaria.Migrations
{
    public partial class AddRecebedorIdAndQuantidadeSolicitada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Solicitacoes",
                newName: "RecebedorId");

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeSolicitada",
                table: "Solicitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_DoacaoId",
                table: "Solicitacoes",
                column: "DoacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_RecebedorId",
                table: "Solicitacoes",
                column: "RecebedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Doacoes_DoacaoId",
                table: "Solicitacoes",
                column: "DoacaoId",
                principalTable: "Doacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Recebedores_RecebedorId",
                table: "Solicitacoes",
                column: "RecebedorId",
                principalTable: "Recebedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Doacoes_DoacaoId",
                table: "Solicitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Recebedores_RecebedorId",
                table: "Solicitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacoes_DoacaoId",
                table: "Solicitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacoes_RecebedorId",
                table: "Solicitacoes");

            migrationBuilder.DropColumn(
                name: "QuantidadeSolicitada",
                table: "Solicitacoes");

            migrationBuilder.RenameColumn(
                name: "RecebedorId",
                table: "Solicitacoes",
                newName: "UsuarioId");
        }
    }
}
