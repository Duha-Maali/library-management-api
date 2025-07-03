using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToBorrowTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Borrows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Borrows",
                keyColumn: "BorrowId",
                keyValue: 1,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Borrows",
                keyColumn: "BorrowId",
                keyValue: 2,
                columns: new[] { "ReturnDate", "Status", "UserId" },
                values: new object[] { null, 0, 1 });

            migrationBuilder.UpdateData(
                table: "Borrows",
                keyColumn: "BorrowId",
                keyValue: 3,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$zdwrPf88HygS8pzTFVPQXOVbw4vmHdjqGv5q05a8OSHrkq1LrBBxu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$5sKy3puSOilQ4cZs2Z6X2O5Ia1v0YQt8x0H3fq1G8zqjXWN53cYki");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_UserId",
                table: "Borrows",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_UserId",
                table: "Borrows",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_UserId",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_UserId",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Borrows");

            migrationBuilder.UpdateData(
                table: "Borrows",
                keyColumn: "BorrowId",
                keyValue: 2,
                columns: new[] { "ReturnDate", "Status" },
                values: new object[] { new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$N4ghIO/ZIBpSQkG0/IW3wuF4uSo9YK8z1E26UIoYIyfsN01LtjzGy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$hKKfEl4ad1CoCckmKMLGS.QBpGf7eINudE2wJaRYQ8ieY/NzaulZm");
        }
    }
}
