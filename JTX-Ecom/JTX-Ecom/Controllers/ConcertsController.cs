using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JTX_Ecom.Data;
using JTX_Ecom.Models;

namespace JTX_Ecom.Controllers
{
    /// <summary>
    /// Controller for managing concert-related views and actions
    /// </summary>
    public class ConcertsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ConcertsController> _logger;

        public ConcertsController(ApplicationDbContext context, ILogger<ConcertsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Display all active concerts with venue information
        /// GET: /Concerts
        /// </summary>
        public async Task<IActionResult> Index(string? genre, string? search, string? sortBy)
        {
            try
            {
                // Start with all active concerts including venue data
                // TEMPORARILY REMOVED DATE FILTER FOR DEBUGGING
                var concertsQuery = _context.Concerts
                    .Include(c => c.Venue)
                    .Where(c => c.IsActive); // Show all active concerts regardless of date

                // Filter by genre if provided
                if (!string.IsNullOrEmpty(genre))
                {
                    concertsQuery = concertsQuery.Where(c => c.Genre == genre);
                }

                // Search by title or artist if provided
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Trim();
                    concertsQuery = concertsQuery.Where(c => 
                        c.Title.Contains(search) || 
                        c.Artist.Contains(search) ||
                        c.Description.Contains(search));
                }

                // Apply sorting
                concertsQuery = sortBy?.ToLower() switch
                {
                    "price-low" => concertsQuery.OrderBy(c => c.BasePrice),
                    "price-high" => concertsQuery.OrderByDescending(c => c.BasePrice),
                    "popular" => concertsQuery.OrderByDescending(c => c.TotalTickets - c.AvailableTickets), // Most tickets sold
                    _ => concertsQuery.OrderBy(c => c.EventDate) // Default: sort by date
                };

                var concerts = await concertsQuery.ToListAsync();

                // Get distinct genres for filter dropdown (from all active concerts)
                ViewBag.Genres = await _context.Concerts
                    .Where(c => c.IsActive)
                    .Select(c => c.Genre)
                    .Distinct()
                    .OrderBy(g => g)
                    .ToListAsync();

                ViewBag.CurrentGenre = genre;
                ViewBag.CurrentSearch = search;
                ViewBag.CurrentSort = sortBy;

                // Log for debugging
                _logger.LogInformation($"Found {concerts.Count} concerts. Current time: {DateTime.Now}");
                if (concerts.Any())
                {
                    _logger.LogInformation($"First concert: {concerts.First().Title} on {concerts.First().EventDate}");
                }

                return View(concerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading concerts");
                TempData["ErrorMessage"] = "An error occurred while loading concerts. Please try again.";
                return View(new List<Concert>());
            }
        }

        /// <summary>
        /// Display detailed information about a specific concert
        /// GET: /Concerts/Details/5
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var concert = await _context.Concerts
                    .Include(c => c.Venue)
                    .FirstOrDefaultAsync(c => c.ConcertId == id);

                if (concert == null)
                {
                    return NotFound();
                }

                // Calculate availability percentage
                ViewBag.AvailabilityPercent = concert.TotalTickets > 0 
                    ? Math.Round((decimal)concert.AvailableTickets / concert.TotalTickets * 100, 1)
                    : 0;

                // Get related concerts (same genre or same venue)
                var relatedConcerts = await _context.Concerts
                    .Include(c => c.Venue)
                    .Where(c => c.IsActive 
                           && c.ConcertId != id 
                           && (c.Genre == concert.Genre || c.VenueId == concert.VenueId))
                    .OrderBy(c => c.EventDate)
                    .Take(3)
                    .ToListAsync();

                ViewBag.RelatedConcerts = relatedConcerts;

                return View(concert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading concert details for ID {ConcertId}", id);
                TempData["ErrorMessage"] = "An error occurred while loading concert details.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
