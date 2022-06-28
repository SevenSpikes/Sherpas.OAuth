using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sherpas.OAuth.Migrations
{
    public partial class Initial_Auth_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auth_User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_AuthenticationCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Consumed = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_AuthenticationCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_AuthenticationCode_Auth_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Auth_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auth_TokenInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_TokenInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auth_TokenInfo_Auth_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Auth_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auth_AuthenticationCode_UserId",
                table: "Auth_AuthenticationCode",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Auth_TokenInfo_UserId",
                table: "Auth_TokenInfo",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auth_AuthenticationCode");

            migrationBuilder.DropTable(
                name: "Auth_TokenInfo");

            migrationBuilder.DropTable(
                name: "Auth_User");
        }
    }
}
