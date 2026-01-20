using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VolumeWeb.Models;

public class AlbumFavorite
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }

    public int AlbumId { get; set; }
    public Album? Album { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
