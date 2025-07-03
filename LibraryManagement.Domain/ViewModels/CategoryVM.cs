
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Business.ViewModels;

public class CategoryVM
{
    public int CategoryId { get; set; }

    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = null!;
}