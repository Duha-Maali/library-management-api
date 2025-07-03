using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Business.ViewModels;

public class UpdateBorrowVM
{
    [Required]
    public DateTime ReturnDate { get; set; }
}
