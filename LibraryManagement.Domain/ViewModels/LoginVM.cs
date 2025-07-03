using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Business.ViewModels;

public class LoginVM
{
    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string Password { get; set; } = null!;
}
