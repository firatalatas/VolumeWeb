using System.ComponentModel.DataAnnotations;

namespace VolumeWeb.Models;

public class Artist
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string Bio { get; set; } = string.Empty;
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public ICollection<Album> Albums { get; set; } = new List<Album>();
}
