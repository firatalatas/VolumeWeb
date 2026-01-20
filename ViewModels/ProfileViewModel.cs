using VolumeWeb.Models;

namespace VolumeWeb.ViewModels;

public class ProfileViewModel
{
    public List<AlbumFavorite> FavoriteAlbums { get; set; } = new();
    public List<ArtistFavorite> FavoriteArtists { get; set; } = new();
    public List<AlbumRating> RatedAlbums { get; set; } = new();
    
    public int TotalFavoriteAlbums => FavoriteAlbums.Count;
    public int TotalFavoriteArtists => FavoriteArtists.Count;
}
