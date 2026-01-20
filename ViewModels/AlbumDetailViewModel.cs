using VolumeWeb.Models;

namespace VolumeWeb.ViewModels;

public class AlbumDetailViewModel
{
    public Album Album { get; set; } = new();
    public bool IsFavorited { get; set; }
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }
    public int? UserRating { get; set; }
}
