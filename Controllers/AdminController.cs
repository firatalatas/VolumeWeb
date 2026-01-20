using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VolumeWeb.Data;
using VolumeWeb.Models;
using VolumeWeb.ViewModels;

namespace VolumeWeb.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new AdminDashboardViewModel
        {
            Artists = await _context.Artists.ToListAsync(),
            Albums = await _context.Albums.Include(a => a.Artists).ToListAsync()
        };
        return View(viewModel);
    }

    [HttpGet]
    public IActionResult CreateArtist()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateArtist(Artist artist)
    {
        if (ModelState.IsValid)
        {
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(artist);
    }

    [HttpGet]
    public async Task<IActionResult> EditArtist(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var artist = await _context.Artists.FindAsync(id);
        if (artist == null)
        {
            return NotFound();
        }
        return View(artist);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditArtist(int id, Artist artist)
    {
        if (id != artist.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(artist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(artist.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(artist);
    }

    [HttpGet]
    public IActionResult CreateAlbum()
    {
        ViewData["Artists"] = new MultiSelectList(_context.Artists, "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAlbum(Album album, int[] artistIds)
    {
        if (ModelState.IsValid)
        {
            if (artistIds != null)
            {
                foreach (var artistId in artistIds)
                {
                    var artist = await _context.Artists.FindAsync(artistId);
                    if (artist != null)
                    {
                        album.Artists.Add(artist);
                    }
                }
            }
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["Artists"] = new MultiSelectList(_context.Artists, "Id", "Name", artistIds);
        return View(album);
    }
    [HttpGet]
    public async Task<IActionResult> EditAlbum(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var album = await _context.Albums.Include(a => a.Artists).FirstOrDefaultAsync(x => x.Id == id);
        if (album == null)
        {
            return NotFound();
        }
        ViewData["Artists"] = new MultiSelectList(_context.Artists, "Id", "Name", album.Artists.Select(a => a.Id));
        return View(album);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAlbum(int id, Album album, int[] artistIds)
    {
        if (id != album.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var albumToUpdate = await _context.Albums.Include(a => a.Artists).FirstOrDefaultAsync(a => a.Id == id);
                
                if (albumToUpdate == null) {
                    return NotFound();
                }

                albumToUpdate.Title = album.Title;
                albumToUpdate.ReleaseYear = album.ReleaseYear;
                albumToUpdate.ReleaseDate = album.ReleaseDate;
                albumToUpdate.Label = album.Label;
                albumToUpdate.Runtime = album.Runtime;
                albumToUpdate.Description = album.Description;
                albumToUpdate.CoverUrl = album.CoverUrl;

                // Update artists
                albumToUpdate.Artists.Clear();
                if (artistIds != null)
                {
                    foreach (var artistId in artistIds)
                    {
                        var artist = await _context.Artists.FindAsync(artistId);
                        if (artist != null)
                        {
                            albumToUpdate.Artists.Add(artist);
                        }
                    }
                }

                _context.Update(albumToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(album.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["Artists"] = new MultiSelectList(_context.Artists, "Id", "Name", artistIds);
        return View(album);
    }

    private bool ArtistExists(int id)
    {
        return _context.Artists.Any(e => e.Id == id);
    }

    private bool AlbumExists(int id)
    {
        return _context.Albums.Any(e => e.Id == id);
    }
}
