
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Business.ViewModels;

public class BorrowerVM
{
    public int BorrowerId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(15)]
    public string Phone { get; set; } = null!;
}
