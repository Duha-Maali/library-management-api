using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Business.ViewModels;

public class CreateCategoryVM
{
    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = null!;
}
