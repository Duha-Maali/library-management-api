using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Borrowers",
                columns: table => new
                {
                    BorrowerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowers", x => x.BorrowerId);
                    table.ForeignKey(
                        name: "FK_Borrowers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Author = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISBN = table.Column<string>(type: "varchar(13)", maxLength: 13, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    TotalCopies = table.Column<int>(type: "int", nullable: false),
                    BorrowedCopies = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Books_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Borrows",
                columns: table => new
                {
                    BorrowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BorrowerId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrows", x => x.BorrowId);
                    table.ForeignKey(
                        name: "FK_Borrows_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Borrows_Borrowers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrowers",
                        principalColumn: "BorrowerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Borrows_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Librarian" },
                    { 2, "Library Cataloger" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Password", "RoleId", "UserName" },
                values: new object[,]
                {
                    { 1, "$2a$11$eNbkjG4uP/Uj7Ag/rmojUuC50bQ4iPkLu94W267Xkhz3l9NtJuPSC", 1, "Duha" },
                    { 2, "$2a$11$ltPc.60t4NPtDmk5FOhfp.yz0Ht.NxhZ/k7guKYi36b1y.n4K.IGG", 2, "Ahmed" }
                });

            migrationBuilder.InsertData(
                table: "Borrowers",
                columns: new[] { "BorrowerId", "Name", "Phone", "UserId" },
                values: new object[,]
                {
                    { 1, "Ahmad Khaled", "1234567890", 1 },
                    { 2, "Sara Ali", "0987654321", 1 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "UserId" },
                values: new object[,]
                {
                    { 1, "Law", 2 },
                    { 2, "History", 2 },
                    { 3, "Education", 2 }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "BorrowedCopies", "CategoryId", "ISBN", "PublishedDate", "Title", "TotalCopies", "UserId" },
                values: new object[,]
                {
                    { 1, "Harper Lee", 1, 3, "9780446310788", new DateTime(1960, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Introduction to Database Systems", 5, 2 },
                    { 2, "Sami Omar", 1, 1, "9780062316097", new DateTime(2011, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Law of Palestine", 3, 2 },
                    { 3, "Ahmed Khaled", 1, 2, "9780553380163", new DateTime(1988, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The History of Palestine", 4, 2 }
                });

            migrationBuilder.InsertData(
                table: "Borrows",
                columns: new[] { "BorrowId", "BookId", "BorrowDate", "BorrowerId", "DueDate", "ReturnDate", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1 },
                    { 2, 2, new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2025, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 1 },
                    { 3, 3, new DateTime(2025, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserId",
                table: "Books",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowers_UserId",
                table: "Borrowers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BookId",
                table: "Borrows",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BorrowerId",
                table: "Borrows",
                column: "BorrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_UserId",
                table: "Borrows",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Borrows");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Borrowers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
