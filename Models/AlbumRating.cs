using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VolumeWeb.Models;

public class AlbumRating
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }

    public int AlbumId { get; set; }
    public Album? Album { get; set; }

    [Range(1, 16)]
    public int Value { get; set; } // 1-16 strictly

    public DateTime RatedAt { get; set; } = DateTime.Now;
}
