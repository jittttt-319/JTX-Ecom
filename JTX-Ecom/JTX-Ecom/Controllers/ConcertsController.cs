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
        public async Task<IActionResult> Index(string? genre, string? search)
        {
            try
            {
                // Start with all active concerts including venue data
                var concertsQuery = _context.Concerts
                    .Include(c => c.Venue)
                    .Where(c => c.IsActive);

                // Filter by genre if provided
                if (!string.IsNullOrEmpty(genre))
                {
                    concertsQuery = concertsQuery.Where(c => c.Genre == genre);
                }

                // Search by title or artist if provided
                if (!string.IsNullOrEmpty(search))
                {
                    concertsQuery = concertsQuery.Where(c => 
                        c.Title.Contains(search) || 
                        c.Artist.Contains(search));
                }

                // Order by event date
                var concerts = await concertsQuery
                    .OrderBy(c => c.EventDate)
                    .ToListAsync();

                // Get distinct genres for filter dropdown
                ViewBag.Genres = await _context.Concerts
                    .Where(c => c.IsActive)
                    .Select(c => c.Genre)
                    .Distinct()
                    .OrderBy(g => g)
                    .ToListAsync();

                ViewBag.CurrentGenre = genre;
                ViewBag.CurrentSearch = search;

                return View(concerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading concerts");
                return View("Error");
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

                return View(concert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading concert details for ID {ConcertId}", id);
                return View("Error");
            }
        }
    }
}
