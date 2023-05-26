using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TimeGuardian_API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SessionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Login = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SessionTypeId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_SessionTypes_SessionTypeId",
                        column: x => x.SessionTypeId,
                        principalTable: "SessionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "SessionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Nauka" },
                    { 2, "Praca" },
                    { 3, "Prokrastynacja" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Login", "Password" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 26, 12, 25, 24, 521, DateTimeKind.Local).AddTicks(3530), "admin@admin.com", "Admin", "Admin1" },
                    { 2, new DateTime(2023, 5, 26, 12, 25, 24, 521, DateTimeKind.Local).AddTicks(3555), "jan@jan.com", "Jan", "Jan1" }
                });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "Duration", "EndTime", "SessionTypeId", "StartTime", "UserId" },
                values: new object[,]
                {
                    { 1, 6339, new DateTime(2023, 5, 17, 18, 58, 13, 0, DateTimeKind.Unspecified), 1, new DateTime(2023, 5, 17, 17, 12, 34, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 3600, new DateTime(2023, 5, 17, 19, 58, 13, 0, DateTimeKind.Unspecified), 3, new DateTime(2023, 5, 17, 18, 58, 13, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 15787, new DateTime(2023, 5, 17, 23, 47, 3, 0, DateTimeKind.Unspecified), 3, new DateTime(2023, 5, 17, 19, 23, 56, 0, DateTimeKind.Unspecified), 1 },
                    { 4, 15787, new DateTime(2023, 5, 17, 23, 47, 3, 0, DateTimeKind.Unspecified), 1, new DateTime(2023, 5, 17, 19, 23, 56, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SessionTypeId",
                table: "Sessions",
                column: "SessionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "SessionTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
