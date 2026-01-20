using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolumeWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VolumeWeb.ViewModels;

namespace VolumeWeb.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound("User not found");

        var favoriteAlbums = await _context.AlbumFavorites
            .Include(f => f.Album)
            .ThenInclude(a => a.Artists)
            .Where(f => f.UserId == user.Id)
            .ToListAsync();

        var favoriteArtists = await _context.ArtistFavorites
            .Include(f => f.Artist)
            .Where(f => f.UserId == user.Id)
            .ToListAsync();

        var ratedAlbums = await _context.AlbumRatings
            .Include(r => r.Album)
            .ThenInclude(a => a.Artists)
            .Where(r => r.UserId == user.Id)
            .OrderByDescending(r => r.RatedAt)
            .ToListAsync();

        var viewModel = new ProfileViewModel
        {
            FavoriteAlbums = favoriteAlbums,
            FavoriteArtists = favoriteArtists,
            RatedAlbums = ratedAlbums
        };

        return View(viewModel);
    }
}
