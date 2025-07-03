using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Data.Models;

public class Book
{
    [Key]
    public int BookId { get; set; }

    [Required]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Author { get; set; } = null!;

    [StringLength(13)]
    public string? ISBN { get; set; }

    public DateTime? PublishedDate { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int TotalCopies { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int AvailableCopies { get; set; }

    //[Required]
    //public int UserId { get; set; }

    [ForeignKey("CategoryId")]
    public Category Category { get; set; } = null!;

    //[ForeignKey("UserId")]
    //public User User { get; set; } = null!;
}
