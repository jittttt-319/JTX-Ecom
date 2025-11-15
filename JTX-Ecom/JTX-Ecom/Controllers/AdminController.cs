using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JTX_Ecom.Data;
using JTX_Ecom.Models;
using JTX_Ecom.Models.DTOs;

namespace JTX_Ecom.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            var stats = new
            {
                TotalConcerts = await _context.Concerts.CountAsync(),
                ActiveConcerts = await _context.Concerts.CountAsync(c => c.IsActive),
                TotalVenues = await _context.Venues.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalRevenue = await _context.Orders
                    .Where(o => o.PaymentStatus == "Completed")
                    .SumAsync(o => o.TotalAmount)
            };

            ViewBag.Stats = stats;
            return View();
        }

        // Concerts Management
        public async Task<IActionResult> Concerts()
        {
            var concerts = await _context.Concerts
                .Include(c => c.Venue)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(concerts);
        }

        // Create Concert - GET
        public async Task<IActionResult> CreateConcert()
        {
            ViewBag.Venues = await _context.Venues.ToListAsync();
            return View();
        }

        // Create Concert - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConcert(ConcertDto concertDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Venues = await _context.Venues.ToListAsync();
                return View(concertDto);
            }

            try
            {
                var concert = new Concert
                {
                    Title = concertDto.Title,
                    Description = concertDto.Description,
                    EventDate = concertDto.EventDate,
                    EventTime = DateTime.Parse(concertDto.EventTime),
                    Artist = concertDto.Artist,
                    Genre = concertDto.Genre,
                    ImageUrl = concertDto.ImageUrl,
                    BasePrice = concertDto.BasePrice,
                    AvailableTickets = concertDto.AvailableTickets,
                    TotalTickets = concertDto.TotalTickets,
                    IsActive = concertDto.IsActive,
                    VenueId = concertDto.VenueId,
                    CreatedAt = DateTime.Now
                };

                _context.Concerts.Add(concert);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Concert created successfully!";
                return RedirectToAction(nameof(Concerts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating concert");
                ModelState.AddModelError("", "An error occurred while creating the concert");
                ViewBag.Venues = await _context.Venues.ToListAsync();
                return View(concertDto);
            }
        }

        // Edit Concert - GET
        public async Task<IActionResult> EditConcert(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.Concerts.FindAsync(id);
            if (concert == null)
            {
                return NotFound();
            }

            var concertDto = new ConcertDto
            {
                ConcertId = concert.ConcertId,
                Title = concert.Title,
                Description = concert.Description,
                EventDate = concert.EventDate,
                EventTime = concert.EventTime.ToString("HH:mm"),
                Artist = concert.Artist,
                Genre = concert.Genre,
                ImageUrl = concert.ImageUrl,
                BasePrice = concert.BasePrice,
                AvailableTickets = concert.AvailableTickets,
                TotalTickets = concert.TotalTickets,
                IsActive = concert.IsActive,
                VenueId = concert.VenueId
            };

            ViewBag.Venues = await _context.Venues.ToListAsync();
            return View(concertDto);
        }

        // Edit Concert - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConcert(int id, ConcertDto concertDto)
        {
            if (id != concertDto.ConcertId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Venues = await _context.Venues.ToListAsync();
                return View(concertDto);
            }

            try
            {
                var concert = await _context.Concerts.FindAsync(id);
                if (concert == null)
                {
                    return NotFound();
                }

                concert.Title = concertDto.Title;
                concert.Description = concertDto.Description;
                concert.EventDate = concertDto.EventDate;
                concert.EventTime = DateTime.Parse(concertDto.EventTime);
                concert.Artist = concertDto.Artist;
                concert.Genre = concertDto.Genre;
                concert.ImageUrl = concertDto.ImageUrl;
                concert.BasePrice = concertDto.BasePrice;
                concert.AvailableTickets = concertDto.AvailableTickets;
                concert.TotalTickets = concertDto.TotalTickets;
                concert.IsActive = concertDto.IsActive;
                concert.VenueId = concertDto.VenueId;
                concert.UpdatedAt = DateTime.Now;

                _context.Update(concert);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Concert updated successfully!";
                return RedirectToAction(nameof(Concerts));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ConcertExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating concert");
                ModelState.AddModelError("", "An error occurred while updating the concert");
                ViewBag.Venues = await _context.Venues.ToListAsync();
                return View(concertDto);
            }
        }

        // Delete Concert - GET
        public async Task<IActionResult> DeleteConcert(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.Concerts
                .Include(c => c.Venue)
                .FirstOrDefaultAsync(c => c.ConcertId == id);

            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        // Delete Concert - POST
        [HttpPost, ActionName("DeleteConcert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConcertConfirmed(int id)
        {
            try
            {
                var concert = await _context.Concerts.FindAsync(id);
                if (concert == null)
                {
                    return NotFound();
                }

                _context.Concerts.Remove(concert);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Concert deleted successfully!";
                return RedirectToAction(nameof(Concerts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting concert");
                TempData["ErrorMessage"] = "An error occurred while deleting the concert";
                return RedirectToAction(nameof(Concerts));
            }
        }

        // Venues Management
        public async Task<IActionResult> Venues()
        {
            var venues = await _context.Venues
                .Include(v => v.Concerts)
                .ToListAsync();
            return View(venues);
        }

        // Orders Management
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.Tickets)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        private async Task<bool> ConcertExists(int id)
        {
            return await _context.Concerts.AnyAsync(c => c.ConcertId == id);
        }
    }
}
