using LibraryManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data.Helpers;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrower> Borrowers { get; set; }
    public DbSet<Borrow> Borrows { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Category
        modelBuilder.Entity<Category>()
            .HasKey(c => c.CategoryId);
        modelBuilder.Entity<Category>()
            .Property(c => c.CategoryName)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(true);
        //modelBuilder.Entity<Category>()
        //    .HasOne(c => c.User)
        //    .WithMany()
        //    .HasForeignKey(c => c.UserId)
        //    .OnDelete(DeleteBehavior.Restrict);

        // Configure Book
        modelBuilder.Entity<Book>()
            .HasKey(b => b.BookId);
        modelBuilder.Entity<Book>()
            .Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(255);
        modelBuilder.Entity<Book>()
            .Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<Book>()
            .Property(b => b.ISBN)
            .HasMaxLength(13);
        modelBuilder.Entity<Book>()
            .Property(b => b.TotalCopies)
            .IsRequired();
        modelBuilder.Entity<Book>()
            .Property(b => b.AvailableCopies)
            .IsRequired();
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        //modelBuilder.Entity<Book>()
        //    .HasOne(b => b.User)
        //    .WithMany()
        //    .HasForeignKey(b => b.UserId)
        //    .OnDelete(DeleteBehavior.Restrict);

        // Configure Borrower
        modelBuilder.Entity<Borrower>()
            .HasKey(b => b.BorrowerId);
        modelBuilder.Entity<Borrower>()
            .Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<Borrower>()
            .Property(b => b.Phone)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);
        //modelBuilder.Entity<Borrower>()
        //    .HasOne(b => b.User)
        //    .WithMany()
        //    .HasForeignKey(b => b.UserId)
        //    .OnDelete(DeleteBehavior.Restrict);

        // Configure Borrow
        modelBuilder.Entity<Borrow>()
            .HasKey(b => b.BorrowId);
        modelBuilder.Entity<Borrow>()
            .Property(b => b.BorrowDate)
            .IsRequired();
        modelBuilder.Entity<Borrow>()
            .Property(b => b.DueDate)
            .IsRequired();
        modelBuilder.Entity<Borrow>()
            .Property(b => b.Status)
            .IsRequired();
        modelBuilder.Entity<Borrow>()
            .HasOne(b => b.Borrower)
            .WithMany()
            .HasForeignKey(b => b.BorrowerId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Borrow>()
            .HasOne(b => b.Book)
            .WithMany()
            .HasForeignKey(b => b.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Borrow>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure User
        modelBuilder.Entity<User>()
            .HasKey(a => a.UserId);
        modelBuilder.Entity<User>()
            .Property(a => a.UserName)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<User>()
            .Property(a => a.Password)
            .IsRequired()
            .HasMaxLength(255);
        // Add unique index on UserName
        modelBuilder.Entity<User>()
            .HasIndex(a => a.UserName)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Role
        modelBuilder.Entity<Role>()
            .HasKey(r => r.RoleId);
        modelBuilder.Entity<Role>()
            .Property(r => r.RoleName)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.RoleName)
            .IsUnique();

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, CategoryName = "Law"},
            new Category { CategoryId = 2, CategoryName = "History"},
            new Category { CategoryId = 3, CategoryName = "Education"}
        );
        //modelBuilder.Entity<Category>().HasData(
        //    new Category { CategoryId = 1, CategoryName = "Law", UserId = 2 },
        //    new Category { CategoryId = 2, CategoryName = "History", UserId = 2 },
        //    new Category { CategoryId = 3, CategoryName = "Education", UserId = 2 }
        //);

        // Seed Books
        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                BookId = 1,
                Title = "Introduction to Database Systems",
                Author = "Harper Lee",
                ISBN = "9780446310788",
                PublishedDate = new DateTime(1960, 7, 11),
                CategoryId = 3,
                TotalCopies = 5,
                AvailableCopies = 4
            },
            new Book
            {
                BookId = 2,
                Title = "Law of Palestine",
                Author = "Sami Omar",
                ISBN = "9780062316097",
                PublishedDate = new DateTime(2011, 2, 10),
                CategoryId = 1,
                TotalCopies = 3,
                AvailableCopies = 2
            },
            new Book
            {
                BookId = 3,
                Title = "The History of Palestine",
                Author = "Ahmed Khaled",
                ISBN = "9780553380163",
                PublishedDate = new DateTime(1988, 3, 1),
                CategoryId = 2,
                TotalCopies = 4,
                AvailableCopies = 3
            }
        );
        //modelBuilder.Entity<Book>().HasData(
        //    new Book
        //    {
        //        BookId = 1,
        //        Title = "Introduction to Database Systems",
        //        Author = "Harper Lee",
        //        ISBN = "9780446310788",
        //        PublishedDate = new DateTime(1960, 7, 11),
        //        CategoryId = 3,
        //        TotalCopies = 5,
        //        AvailableCopies = 4,
        //        UserId = 2
        //    },
        //    new Book
        //    {
        //        BookId = 2,
        //        Title = "Law of Palestine",
        //        Author = "Sami Omar",
        //        ISBN = "9780062316097",
        //        PublishedDate = new DateTime(2011, 2, 10),
        //        CategoryId = 1,
        //        TotalCopies = 3,
        //        AvailableCopies = 4,
        //        UserId = 2
        //    },
        //    new Book
        //    {
        //        BookId = 3,
        //        Title = "The History of Palestine",
        //        Author = "Ahmed Khaled",
        //        ISBN = "9780553380163",
        //        PublishedDate = new DateTime(1988, 3, 1),
        //        CategoryId = 2,
        //        TotalCopies = 4,
        //        AvailableCopies = 1,
        //        UserId = 2
        //    }
        //);

        // Seed Borrowers
        modelBuilder.Entity<Borrower>().HasData(
            new Borrower { BorrowerId = 1, Name = "Ahmad Khaled", Phone = "1234567890"},
            new Borrower { BorrowerId = 2, Name = "Sara Ali", Phone = "0987654321"}
        );
        //modelBuilder.Entity<Borrower>().HasData(
        //    new Borrower { BorrowerId = 1, Name = "Ahmad Khaled", Phone = "1234567890", UserId = 1 },
        //    new Borrower { BorrowerId = 2, Name = "Sara Ali", Phone = "0987654321", UserId = 1 }
        //);

        // Seed Borrows
        modelBuilder.Entity<Borrow>().HasData(
            new Borrow
            {
                BorrowId = 1,
                BorrowerId = 1,
                BookId = 1,
                BorrowDate = new DateTime(2025, 6, 1),
                DueDate = new DateTime(2025, 6, 15),
                ReturnDate = null,
                Status = BorrowStatus.Overdue,
                UserId = 1
            },
            new Borrow
            {
                BorrowId = 2,
                BorrowerId = 2,
                BookId = 2,
                BorrowDate = new DateTime(2025, 5, 20),
                DueDate = new DateTime(2025, 6, 3),
                ReturnDate = null,
                Status = BorrowStatus.Borrowed,
                UserId = 1
            },
            new Borrow
            {
                BorrowId = 3,
                BorrowerId = 1,
                BookId = 3,
                BorrowDate = new DateTime(2025, 6, 25),
                DueDate = new DateTime(2025, 7, 17),
                ReturnDate = null,
                Status = BorrowStatus.Borrowed,
                UserId = 1
            }
        );


        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "Librarian" },
            new Role { RoleId = 2, RoleName = "Library Cataloger" }
        );

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                UserName = "Duha",
                Password = BCrypt.Net.BCrypt.HashPassword("duha@12345"),
                RoleId = 1 // Librarian
            },
            new User
            {
                UserId = 2,
                UserName = "Ahmed",
                Password = BCrypt.Net.BCrypt.HashPassword("123"),
                RoleId = 2 // Library Cataloger
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}
