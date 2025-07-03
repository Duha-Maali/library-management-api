using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Data.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    [Required]
    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    public Role Role { get; set; } = null!;
}
