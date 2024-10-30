using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(64)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    Password = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    UserIdentifier = table.Column<string>(type: "VARCHAR(128)", nullable: false),
                    Role = table.Column<string>(type: "VARCHAR(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    Id = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "BIGINT", nullable: false),
                    Title = table.Column<string>(type: "VARCHAR(64)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(256)", nullable: true),
                    Date = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Amount = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    PaymentType = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expense_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expense_UserId",
                table: "Expense",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
