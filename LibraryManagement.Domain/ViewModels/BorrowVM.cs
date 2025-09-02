using System.ComponentModel.DataAnnotations;
using LibraryManagement.Data.Models;

namespace LibraryManagement.Business.ViewModels;
public class BorrowVM
{
    public int BorrowId { get; set; }

    [Required]
    public string BorrowerName { get; set; } = null!;

    [Required]
    public string BookTitle { get; set; } = null!;

    [Required]
    public DateTime BorrowDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    [Required]
    [RegularExpression("^(Borrowed|Returned|Overdue|Archived)$", ErrorMessage = "Status must be 'Borrowed', 'Returned', 'Overdue', or 'Archived'.")]
    public string Status { get; set; } = null!;
}
