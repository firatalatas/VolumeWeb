using VolumeWeb.Models;

namespace VolumeWeb.ViewModels;

public class AdminDashboardViewModel
{
    public List<Artist> Artists { get; set; } = new List<Artist>();
    public List<Album> Albums { get; set; } = new List<Album>();
}
