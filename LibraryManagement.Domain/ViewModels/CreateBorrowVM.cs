using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Business.ViewModels;

public class CreateBorrowVM
{
    [Required]
    [Range(1, int.MaxValue)]
    public int BorrowerId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int BookId { get; set; }

    [Required]
    public DateTime BorrowDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }
}
