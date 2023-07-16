using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatersRating.Migrations
{
    /// <inheritdoc />
    public partial class resto3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "Amizade",
                newName: "AmigoId");

            migrationBuilder.CreateTable(
                name: "UsuariosRating",
                columns: table => new
                {
                    RatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosRating", x => x.RatingId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuariosRating");

            migrationBuilder.RenameColumn(
                name: "AmigoId",
                table: "Amizade",
                newName: "RatingId");
        }
    }
}
