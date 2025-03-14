using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Database.Migrations
{
    /// <inheritdoc />
    public partial class updatedeviceaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceActions_Routines_RoutineId1",
                table: "DeviceActions");

            migrationBuilder.DropIndex(
                name: "IX_DeviceActions_RoutineId1",
                table: "DeviceActions");

            migrationBuilder.DropColumn(
                name: "RoutineId1",
                table: "DeviceActions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
