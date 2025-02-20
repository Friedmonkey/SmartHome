using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Database.Migrations
{
    /// <inheritdoc />
    public partial class updateroutineentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Start",
                table: "Routines",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "RepeatDays",
                table: "Routines",
                type: "binary(8)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "binary(7)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Start",
                table: "Routines",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AlterColumn<byte[]>(
                name: "RepeatDays",
                table: "Routines",
                type: "binary(7)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "binary(8)");
        }
    }
}
