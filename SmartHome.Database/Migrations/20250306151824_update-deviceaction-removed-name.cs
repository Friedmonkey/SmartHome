using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Database.Migrations
{
    /// <inheritdoc />
    public partial class updatedeviceactionremovedname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "DeviceActions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DeviceActions",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
