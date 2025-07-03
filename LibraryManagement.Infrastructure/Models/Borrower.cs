using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Data.Models;

public class Borrower
{
    [Key]
    public int BorrowerId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(15)]
    public string Phone { get; set; } = null!;

    //[Required]
    //public int UserId { get; set; }

    //[ForeignKey("UserId")]
    //public User User { get; set; } = null!;
}
