using VolumeWeb.Models;

namespace VolumeWeb.ViewModels;

public class SearchViewModel
{
    public string Query { get; set; }
    public List<Artist> Artists { get; set; } = new List<Artist>();
    public List<Album> Albums { get; set; } = new List<Album>();
}
