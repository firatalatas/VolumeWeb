using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolumeWeb.Data;
using VolumeWeb.Models;
using VolumeWeb.ViewModels;

namespace VolumeWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder)
    {
        ViewData["CurrentSort"] = sortOrder;
        
        var albumsQuery = _context.Albums.Include(a => a.Artists).AsQueryable();

        var albums = await albumsQuery.ToListAsync();

        switch (sortOrder)
        {
            case "Artist":
                albums = albums.OrderBy(a => a.Artists.Min(art => art.Name)).ToList();
                break;
            case "Name":
                albums = albums.OrderBy(a => a.Title.Trim(), StringComparer.OrdinalIgnoreCase).ToList();
                break;
            case "Date":
            default:
                albums = albums.OrderByDescending(a => a.ReleaseDate).ThenByDescending(a => a.ReleaseYear).ToList();
                break;
        }

        return View(albums);
    }

    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return View(new SearchViewModel { Query = query });
        }

        var artists = await _context.Artists
            .Where(a => a.Name.Contains(query))
            .ToListAsync();

        var albums = await _context.Albums
            .Include(a => a.Artists)
            .Where(a => a.Title.Contains(query))
            .ToListAsync();

        var viewModel = new SearchViewModel
        {
            Query = query,
            Artists = artists,
            Albums = albums
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
