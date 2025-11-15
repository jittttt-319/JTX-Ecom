using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JTX_Ecom.Data;
using JTX_Ecom.Models;
using JTX_Ecom.Models.DTOs;

namespace JTX_Ecom.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CartController> _logger;

        public CartController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<CartController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await GetOrCreateCart(userId);
            var cartSummary = await GetCartSummary(cart.CartId);

            return View(cartSummary);
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(AddToCartDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data";
                return RedirectToAction("Details", "Concerts", new { id = model.ConcertId });
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var concert = await _context.Concerts.FindAsync(model.ConcertId);
                if (concert == null)
                {
                    TempData["ErrorMessage"] = "Concert not found";
                    return RedirectToAction("Index", "Concerts");
                }

                if (concert.AvailableTickets < model.Quantity)
                {
                    TempData["ErrorMessage"] = $"Only {concert.AvailableTickets} tickets available";
                    return RedirectToAction("Details", "Concerts", new { id = model.ConcertId });
                }

                var cart = await GetOrCreateCart(userId);

                // Check if item already exists in cart
                var existingItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId 
                        && ci.ConcertId == model.ConcertId 
                        && ci.TicketType == model.TicketType);

                if (existingItem != null)
                {
                    existingItem.Quantity += model.Quantity;
                    existingItem.PricePerTicket = GetTicketPrice(concert.BasePrice, model.TicketType);
                    _context.Update(existingItem);
                }
                else
                {
                    var cartItem = new CartItem
                    {
                        CartId = cart.CartId,
                        ConcertId = model.ConcertId,
                        Quantity = model.Quantity,
                        TicketType = model.TicketType,
                        PricePerTicket = GetTicketPrice(concert.BasePrice, model.TicketType)
                    };

                    _context.CartItems.Add(cartItem);
                }

                cart.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"{model.Quantity} ticket(s) added to cart";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                TempData["ErrorMessage"] = "An error occurred while adding to cart";
                return RedirectToAction("Details", "Concerts", new { id = model.ConcertId });
            }
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(UpdateCartItemDto model)
        {
            try
            {
                var cartItem = await _context.CartItems
                    .Include(ci => ci.Concert)
                    .FirstOrDefaultAsync(ci => ci.CartItemId == model.CartItemId);

                if (cartItem == null)
                {
                    return Json(new { success = false, message = "Cart item not found" });
                }

                if (cartItem.Concert!.AvailableTickets < model.Quantity)
                {
                    return Json(new { success = false, message = $"Only {cartItem.Concert.AvailableTickets} tickets available" });
                }

                cartItem.Quantity = model.Quantity;
                await _context.SaveChangesAsync();

                var userId = _userManager.GetUserId(User);
                var cart = await _context.Carts.FirstAsync(c => c.UserId == userId);
                var cartSummary = await GetCartSummary(cart.CartId);

                return Json(new { 
                    success = true, 
                    totalAmount = cartSummary.TotalAmount.ToString("N2"),
                    subtotal = (cartItem.Quantity * cartItem.PricePerTicket).ToString("N2")
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart quantity");
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        // POST: /Cart/RemoveItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            try
            {
                var cartItem = await _context.CartItems.FindAsync(cartItemId);
                if (cartItem == null)
                {
                    TempData["ErrorMessage"] = "Item not found";
                    return RedirectToAction(nameof(Index));
                }

                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Item removed from cart";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cart item");
                TempData["ErrorMessage"] = "An error occurred";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: /Cart/Checkout
        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems!.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

            var checkoutDto = new CheckoutDto
            {
                CustomerName = $"{user?.FirstName} {user?.LastName}",
                CustomerEmail = user?.Email ?? "",
                CustomerPhone = profile?.PhoneNumber ?? "",
                BillingAddress = profile?.Address ?? "",
                City = profile?.City ?? "",
                State = profile?.State ?? "",
                PostalCode = profile?.PostalCode ?? ""
            };

            var cartSummary = await GetCartSummary(cart.CartId);
            ViewBag.CartSummary = cartSummary;

            // Malaysian states for dropdown
            ViewBag.MalaysianStates = GetMalaysianStates();

            return View(checkoutDto);
        }

        // Helper methods
        private async Task<Cart> GetOrCreateCart(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        private async Task<CartSummaryDto> GetCartSummary(int cartId)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Concert)
                    .ThenInclude(c => c!.Venue)
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            var items = cartItems.Select(ci => new CartItemDto
            {
                CartItemId = ci.CartItemId,
                ConcertId = ci.ConcertId,
                ConcertTitle = ci.Concert?.Title ?? "",
                Artist = ci.Concert?.Artist ?? "",
                EventDate = ci.Concert?.EventDate ?? DateTime.Now,
                VenueName = ci.Concert?.Venue?.Name ?? "",
                TicketType = ci.TicketType,
                Quantity = ci.Quantity,
                PricePerTicket = ci.PricePerTicket,
                Subtotal = ci.Quantity * ci.PricePerTicket,
                ImageUrl = ci.Concert?.ImageUrl
            }).ToList();

            return new CartSummaryDto
            {
                TotalItems = items.Sum(i => i.Quantity),
                TotalAmount = items.Sum(i => i.Subtotal),
                Items = items
            };
        }

        private decimal GetTicketPrice(decimal basePrice, string ticketType)
        {
            return ticketType switch
            {
                "VIP" => basePrice * 2.0m,
                "VVIP" => basePrice * 3.5m,
                _ => basePrice // General
            };
        }

        private List<string> GetMalaysianStates()
        {
            return new List<string>
            {
                "Johor",
                "Kedah",
                "Kelantan",
                "Melaka",
                "Negeri Sembilan",
                "Pahang",
                "Pulau Pinang",
                "Perak",
                "Perlis",
                "Sabah",
                "Sarawak",
                "Selangor",
                "Terengganu",
                "Wilayah Persekutuan Kuala Lumpur",
                "Wilayah Persekutuan Labuan",
                "Wilayah Persekutuan Putrajaya"
            };
        }
    }
}
