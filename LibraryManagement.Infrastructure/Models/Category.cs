using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Data.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = null!;

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
