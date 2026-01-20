using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolumeWeb.Data;
using VolumeWeb.Models;

namespace VolumeWeb.Controllers;

public class ArtistController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ArtistController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var artist = await _context.Artists
            .Include(a => a.Albums)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (artist == null) return NotFound();

        ViewBag.IsFavorited = false;
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.IsFavorited = await _context.ArtistFavorites
                .AnyAsync(f => f.ArtistId == id && f.UserId == userId);
        }

        return View(artist);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> FavoriteToggle(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var existing = await _context.ArtistFavorites
            .FirstOrDefaultAsync(f => f.ArtistId == id && f.UserId == userId);

        bool isFavorited;
        if (existing != null)
        {
            _context.ArtistFavorites.Remove(existing);
            isFavorited = false;
        }
        else
        {
            var favorite = new ArtistFavorite
            {
                ArtistId = id,
                UserId = userId,
                CreatedAt = DateTime.Now
            };
            _context.ArtistFavorites.Add(favorite);
            isFavorited = true;
        }

        await _context.SaveChangesAsync();
        return Ok(new { isFavorited });
    }
}
