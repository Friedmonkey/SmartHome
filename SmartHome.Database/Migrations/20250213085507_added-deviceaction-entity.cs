using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Database.Migrations
{
    /// <inheritdoc />
    public partial class addeddeviceactionentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceAccess_Devices_DeviceId",
                table: "DeviceAccess");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceAccess_SmartUser_SmartUserId",
                table: "DeviceAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceAccess",
                table: "DeviceAccess");

            migrationBuilder.RenameTable(
                name: "DeviceAccess",
                newName: "DeviceAccesses");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceAccess_SmartUserId",
                table: "DeviceAccesses",
                newName: "IX_DeviceAccesses_SmartUserId");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceAccess_DeviceId",
                table: "DeviceAccesses",
                newName: "IX_DeviceAccesses_DeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceAccesses",
                table: "DeviceAccesses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceAccesses_Devices_DeviceId",
                table: "DeviceAccesses",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceAccesses_SmartUser_SmartUserId",
                table: "DeviceAccesses",
                column: "SmartUserId",
                principalTable: "SmartUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceAccesses_Devices_DeviceId",
                table: "DeviceAccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceAccesses_SmartUser_SmartUserId",
                table: "DeviceAccesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceAccesses",
                table: "DeviceAccesses");

            migrationBuilder.RenameTable(
                name: "DeviceAccesses",
                newName: "DeviceAccess");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceAccesses_SmartUserId",
                table: "DeviceAccess",
                newName: "IX_DeviceAccess_SmartUserId");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceAccesses_DeviceId",
                table: "DeviceAccess",
                newName: "IX_DeviceAccess_DeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceAccess",
                table: "DeviceAccess",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceAccess_Devices_DeviceId",
                table: "DeviceAccess",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceAccess_SmartUser_SmartUserId",
                table: "DeviceAccess",
                column: "SmartUserId",
                principalTable: "SmartUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
