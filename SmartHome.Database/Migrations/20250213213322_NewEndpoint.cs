using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome.Database.Migrations
{
    /// <inheritdoc />
    public partial class NewEndpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceAccesses_SmartUser_SmartUserId",
                table: "DeviceAccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceAction_Devices_DeviceId",
                table: "DeviceAction");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceAction_Routine_RoutineId",
                table: "DeviceAction");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Room_RoomId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Log_Home_SmartHomeId",
                table: "Log");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Home_SmartHomeId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Routine_Home_SmartHomeId",
                table: "Routine");

            migrationBuilder.DropTable(
                name: "SmartUser");

            migrationBuilder.DropTable(
                name: "Home");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routine",
                table: "Routine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Log",
                table: "Log");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceAction",
                table: "DeviceAction");

            migrationBuilder.RenameTable(
                name: "Routine",
                newName: "Routines");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "Log",
                newName: "Logs");

            migrationBuilder.RenameTable(
                name: "DeviceAction",
                newName: "DeviceActions");

            migrationBuilder.RenameIndex(
                name: "IX_Routine_SmartHomeId",
                table: "Routines",
                newName: "IX_Routines_SmartHomeId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_SmartHomeId",
                table: "Rooms",
                newName: "IX_Rooms_SmartHomeId");

            migrationBuilder.RenameIndex(
                name: "IX_Log_SmartHomeId",
                table: "Logs",
                newName: "IX_Logs_SmartHomeId");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceAction_RoutineId",
                table: "DeviceActions",
                newName: "IX_DeviceActions_RoutineId");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceAction_DeviceId",
                table: "DeviceActions",
                newName: "IX_DeviceActions_DeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routines",
                table: "Routines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logs",
                table: "Logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceActions",
                table: "DeviceActions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SmartHomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SSID = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SSPassword = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartHomes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SmartUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SmartHomeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartUsers_SmartHomes_SmartHomeId",
                        column: x => x.SmartHomeId,
                        principalTable: "SmartHomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartUsers_Users_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SmartUsers_AccountId",
                table: "SmartUsers",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartUsers_SmartHomeId",
                table: "SmartUsers",
                column: "SmartHomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceAccesses_SmartUsers_SmartUserId",
                table: "DeviceAccesses",
                column: "SmartUserId",
                principalTable: "SmartUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceActions_Devices_DeviceId",
                table: "DeviceActions",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceActions_Routines_RoutineId",
                table: "DeviceActions",
                column: "RoutineId",
                principalTable: "Routines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Rooms_RoomId",
                table: "Devices",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_SmartHomes_SmartHomeId",
                table: "Logs",
                column: "SmartHomeId",
                principalTable: "SmartHomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_SmartHomes_SmartHomeId",
                table: "Rooms",
                column: "SmartHomeId",
                principalTable: "SmartHomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routines_SmartHomes_SmartHomeId",
                table: "Routines",
                column: "SmartHomeId",
                principalTable: "SmartHomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceAccesses_SmartUsers_SmartUserId",
                table: "DeviceAccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceActions_Devices_DeviceId",
                table: "DeviceActions");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceActions_Routines_RoutineId",
                table: "DeviceActions");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Rooms_RoomId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_SmartHomes_SmartHomeId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_SmartHomes_SmartHomeId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Routines_SmartHomes_SmartHomeId",
                table: "Routines");

            migrationBuilder.DropTable(
                name: "SmartUsers");

            migrationBuilder.DropTable(
                name: "SmartHomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routines",
                table: "Routines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logs",
                table: "Logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceActions",
                table: "DeviceActions");

            migrationBuilder.RenameTable(
                name: "Routines",
                newName: "Routine");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameTable(
                name: "Logs",
                newName: "Log");

            migrationBuilder.RenameTable(
                name: "DeviceActions",
                newName: "DeviceAction");

            migrationBuilder.RenameIndex(
                name: "IX_Routines_SmartHomeId",
                table: "Routine",
                newName: "IX_Routine_SmartHomeId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_SmartHomeId",
                table: "Room",
                newName: "IX_Room_SmartHomeId");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_SmartHomeId",
                table: "Log",
                newName: "IX_Log_SmartHomeId");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceActions_RoutineId",
                table: "DeviceAction",
                newName: "IX_DeviceAction_RoutineId");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceActions_DeviceId",
                table: "DeviceAction",
                newName: "IX_DeviceAction_DeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routine",
                table: "Routine",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Log",
                table: "Log",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceAction",
                table: "DeviceAction",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Home",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SSID = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SSPassword = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Home", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SmartUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SmartHomeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartUser_Home_SmartHomeId",
                        column: x => x.SmartHomeId,
                        principalTable: "Home",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartUser_Users_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SmartUser_AccountId",
                table: "SmartUser",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartUser_SmartHomeId",
                table: "SmartUser",
                column: "SmartHomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceAccesses_SmartUser_SmartUserId",
                table: "DeviceAccesses",
                column: "SmartUserId",
                principalTable: "SmartUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceAction_Devices_DeviceId",
                table: "DeviceAction",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceAction_Routine_RoutineId",
                table: "DeviceAction",
                column: "RoutineId",
                principalTable: "Routine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Room_RoomId",
                table: "Devices",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Home_SmartHomeId",
                table: "Log",
                column: "SmartHomeId",
                principalTable: "Home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Home_SmartHomeId",
                table: "Room",
                column: "SmartHomeId",
                principalTable: "Home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routine_Home_SmartHomeId",
                table: "Routine",
                column: "SmartHomeId",
                principalTable: "Home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
