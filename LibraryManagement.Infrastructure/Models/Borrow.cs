using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Data.Models;

public enum BorrowStatus
{
    Borrowed,
    Returned,
    Overdue,
    Inactive
}

public class Borrow
{
    [Key]
    public int BorrowId { get; set; }

    [Required]
    public int BorrowerId { get; set; }

    [Required]
    public int BookId { get; set; }

    [Required]
    public DateTime BorrowDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    [Required]
    public BorrowStatus Status { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("BorrowerId")]
    public Borrower Borrower { get; set; } = null!;

    [ForeignKey("BookId")]
    public Book Book { get; set; } = null!;

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}