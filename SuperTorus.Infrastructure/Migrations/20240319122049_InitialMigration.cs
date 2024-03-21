using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperTorus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Torus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OuterRadius = table.Column<double>(type: "float", nullable: false),
                    InnerRadius = table.Column<double>(type: "float", nullable: false),
                    CenterX = table.Column<double>(type: "float", nullable: false),
                    CenterY = table.Column<double>(type: "float", nullable: false),
                    CenterZ = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Torus", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Torus");
        }
    }
}
