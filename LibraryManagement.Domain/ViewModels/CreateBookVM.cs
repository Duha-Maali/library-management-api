using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryManagement.Business.ViewModels;

public class CreateBookVM
{
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
}
