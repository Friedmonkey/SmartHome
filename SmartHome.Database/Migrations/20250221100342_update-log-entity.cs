using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Database.Migrations
{
    /// <inheritdoc />
    public partial class updatelogentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Logs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(60)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "SmartUserId",
                table: "Logs",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "RoutineId1",
                table: "DeviceActions",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceActions_RoutineId1",
                table: "DeviceActions",
                column: "RoutineId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceActions_Routines_RoutineId1",
                table: "DeviceActions",
                column: "RoutineId1",
                principalTable: "Routines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceActions_Routines_RoutineId1",
                table: "DeviceActions");

            migrationBuilder.DropIndex(
                name: "IX_DeviceActions_RoutineId1",
                table: "DeviceActions");

            migrationBuilder.DropColumn(
                name: "SmartUserId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "RoutineId1",
                table: "DeviceActions");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Logs",
                type: "varchar(60)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
