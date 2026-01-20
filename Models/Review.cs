using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VolumeWeb.Models;

public class Review
{
    public int Id { get; set; }
    
    public int AlbumId { get; set; }
    public Album? Album { get; set; }
    
    // Simplification for prototype: Using string for UserId (simulating IdentityUser later if needed, or just a simple user ID)
    // For now, let's keep it simple.
    [Required]
    public string Username { get; set; } = "Guest";
    
    [Range(1, 16)]
    public int VolumeLevel { get; set; } // 1-16 strictly
    
    public string Comment { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
