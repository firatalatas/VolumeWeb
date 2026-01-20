using System.ComponentModel.DataAnnotations;

namespace VolumeWeb.Models;

public class Album
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public int ReleaseYear { get; set; }

    public DateTime? ReleaseDate { get; set; }
    public string? Label { get; set; }
    public string? Runtime { get; set; }
    public string? Description { get; set; }

    public string CoverUrl { get; set; } = string.Empty;
    
    public ICollection<Artist> Artists { get; set; } = new List<Artist>();
    
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
