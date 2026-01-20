using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VolumeWeb.Models;

public class ArtistFavorite
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }

    public int ArtistId { get; set; }
    public Artist? Artist { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
