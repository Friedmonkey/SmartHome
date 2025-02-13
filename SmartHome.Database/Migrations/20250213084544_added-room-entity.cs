using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Database.Migrations
{
    /// <inheritdoc />
    public partial class addedroomentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartUser_Accounts_AccountId",
                table: "SmartUser");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartUser_Home_SmartHomeId",
                table: "SmartUser");

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SmartHomeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Home_SmartHomeId",
                        column: x => x.SmartHomeId,
                        principalTable: "Home",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Room_SmartHomeId",
                table: "Room",
                column: "SmartHomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartUser_Accounts_AccountId",
                table: "SmartUser",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartUser_Home_SmartHomeId",
                table: "SmartUser",
                column: "SmartHomeId",
                principalTable: "Home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartUser_Accounts_AccountId",
                table: "SmartUser");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartUser_Home_SmartHomeId",
                table: "SmartUser");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartUser_Accounts_AccountId",
                table: "SmartUser",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartUser_Home_SmartHomeId",
                table: "SmartUser",
                column: "SmartHomeId",
                principalTable: "Home",
                principalColumn: "Id");
        }
    }
}
