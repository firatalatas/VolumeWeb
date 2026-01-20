using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolumeWeb.Data;
using VolumeWeb.Models;
using VolumeWeb.ViewModels;

namespace VolumeWeb.Controllers;

public class AlbumController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AlbumController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Album/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var album = await _context.Albums
            .Include(a => a.Artists)
            .Include(a => a.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (album == null) return NotFound();

        var viewModel = new AlbumDetailViewModel
        {
            Album = album
        };

        // Calculate ratings
        var ratings = await _context.AlbumRatings.Where(r => r.AlbumId == id).ToListAsync();
        if (ratings.Any())
        {
            viewModel.AverageRating = ratings.Average(r => r.Value);
            viewModel.RatingCount = ratings.Count;
        }

        // Check favorite status if logged in
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userId = _userManager.GetUserId(User);
            viewModel.IsFavorited = await _context.AlbumFavorites
                .AnyAsync(f => f.AlbumId == id && f.UserId == userId);
            
            var userRating = await _context.AlbumRatings
                .FirstOrDefaultAsync(r => r.AlbumId == id && r.UserId == userId);
            if (userRating != null)
            {
                viewModel.UserRating = userRating.Value;
            }
        }

        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> FavoriteToggle(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var existing = await _context.AlbumFavorites
            .FirstOrDefaultAsync(f => f.AlbumId == id && f.UserId == userId);

        bool isFavorited;
        if (existing != null)
        {
            _context.AlbumFavorites.Remove(existing);
            isFavorited = false;
        }
        else
        {
            var favorite = new AlbumFavorite
            {
                AlbumId = id,
                UserId = userId,
                CreatedAt = DateTime.Now
            };
            _context.AlbumFavorites.Add(favorite);
            isFavorited = true;
        }

        await _context.SaveChangesAsync();
        return Ok(new { isFavorited });
    }

    [HttpPost]
    public async Task<IActionResult> SaveVolume(int albumId, int volumeLevel)
    {
        if (volumeLevel < 1 || volumeLevel > 16) return BadRequest("Invalid volume");

        // New Logic: Use AlbumRating table and Authenticated User
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
             // For guest/legacy support or redirect requirement. 
             // Requirement says: "Login olmayan kullanıcı kalbe tıklarsa login sayfasına yönlendir". 
             // For rating (SaveVolume), the user prompt said "Kullanıcıların albüme puan vermesi/kaydetmesi ile ilgili hiçbir yeni UI ekleme." 
             // But also "Ortalama ve oy sayısı AlbumRating ... üzerinden hesaplanacak".
             // We should enforce Auth for rating to be consistent with the data model.
             return Unauthorized(); 
        }

        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var rating = await _context.AlbumRatings
            .FirstOrDefaultAsync(r => r.AlbumId == albumId && r.UserId == userId);

        if (rating == null)
        {
            rating = new AlbumRating
            {
                AlbumId = albumId,
                UserId = userId,
                Value = volumeLevel,
                RatedAt = DateTime.Now
            };
            _context.AlbumRatings.Add(rating);
        }
        else
        {
            rating.Value = volumeLevel;
            rating.RatedAt = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        
        // Return updated average/count for UI if needed, but existing returning simple success is OK compatible with new logic
        return Ok(new { success = true, level = volumeLevel });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddReview(int albumId, string comment)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        if (string.IsNullOrWhiteSpace(comment)) return BadRequest("Comment cannot be empty");

        // Use the saved rating or default to catch-all if logic requires (though UI shouldn't allow this without rating)
        var rating = await _context.AlbumRatings
             .FirstOrDefaultAsync(r => r.AlbumId == albumId && r.UserId == user.Id);
        
        int currentVolume = rating?.Value ?? 0;

        var review = new Review
        {
            AlbumId = albumId,
            Username = user.UserName, // Using UserName as per model
            VolumeLevel = currentVolume,
            Comment = comment,
            CreatedAt = DateTime.Now
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = albumId });
    }
}
