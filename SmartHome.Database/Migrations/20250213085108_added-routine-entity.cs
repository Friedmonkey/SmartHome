using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Database.Migrations
{
    /// <inheritdoc />
    public partial class addedroutineentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Routine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SmartHomeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RepeatDays = table.Column<byte[]>(type: "binary(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routine_Home_SmartHomeId",
                        column: x => x.SmartHomeId,
                        principalTable: "Home",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Routine_SmartHomeId",
                table: "Routine",
                column: "SmartHomeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Routine");
        }
    }
}
