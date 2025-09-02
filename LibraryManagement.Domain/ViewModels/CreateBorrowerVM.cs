using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Business.ViewModels;

public class CreateBorrowerVM
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(15)]
    public string Phone { get; set; } = null!;
}