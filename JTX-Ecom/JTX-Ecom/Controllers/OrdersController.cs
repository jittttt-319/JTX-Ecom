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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<OrdersController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /Orders
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // GET: /Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var order = await _context.Orders
                .Include(o => o.Tickets)
                    .ThenInclude(t => t!.Concert)
                        .ThenInclude(c => c!.Venue)
                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: /Orders/ProcessCheckout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessCheckout(CheckoutDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields";
                return RedirectToAction("Checkout", "Cart");
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Concert)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.CartItems!.Any())
                {
                    TempData["ErrorMessage"] = "Your cart is empty";
                    return RedirectToAction("Index", "Cart");
                }

                // Calculate total amount
                decimal totalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.PricePerTicket);

                // Create order
                var order = new Order
                {
                    OrderNumber = GenerateOrderNumber(),
                    UserId = userId,
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    BillingAddress = model.BillingAddress,
                    City = model.City,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    TotalAmount = totalAmount,
                    PaymentMethod = model.PaymentMethod,
                    PaymentStatus = "Pending",
                    Quantity = cart.CartItems.Sum(ci => ci.Quantity),
                    OrderDate = DateTime.Now
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Create tickets for each cart item
                foreach (var cartItem in cart.CartItems)
                {
                    for (int i = 0; i < cartItem.Quantity; i++)
                    {
                        var ticket = new Ticket
                        {
                            TicketNumber = GenerateTicketNumber(),
                            ConcertId = cartItem.ConcertId,
                            OrderId = order.OrderId,
                            TicketType = cartItem.TicketType,
                            Price = cartItem.PricePerTicket,
                            Status = "Sold",
                            PurchaseDate = DateTime.Now,
                            QRCode = GenerateQRCode(order.OrderNumber, i + 1)
                        };

                        _context.Tickets.Add(ticket);
                    }

                    // Update concert available tickets
                    var concert = cartItem.Concert;
                    if (concert != null)
                    {
                        concert.AvailableTickets -= cartItem.Quantity;
                        _context.Update(concert);
                    }
                }

                // Clear cart
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();

                // Simulate payment processing
                await ProcessPayment(order, model.PaymentMethod);

                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction(nameof(Confirmation), new { id = order.OrderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing checkout");
                TempData["ErrorMessage"] = "An error occurred while processing your order";
                return RedirectToAction("Checkout", "Cart");
            }
        }

        // GET: /Orders/Confirmation/5
        public async Task<IActionResult> Confirmation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var order = await _context.Orders
                .Include(o => o.Tickets)
                    .ThenInclude(t => t!.Concert)
                        .ThenInclude(c => c!.Venue)
                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            // Group tickets by concert
            var orderItems = order.Tickets!
                .GroupBy(t => t.ConcertId)
                .Select(g => new OrderItemDto
                {
                    ConcertTitle = g.First().Concert?.Title ?? "",
                    Artist = g.First().Concert?.Artist ?? "",
                    EventDate = g.First().Concert?.EventDate ?? DateTime.Now,
                    VenueName = g.First().Concert?.Venue?.Name ?? "",
                    TicketType = g.First().TicketType,
                    Quantity = g.Count(),
                    Price = g.Sum(t => t.Price),
                    TicketNumbers = g.Select(t => t.TicketNumber).ToList()
                })
                .ToList();

            var confirmation = new OrderConfirmationDto
            {
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                PaymentStatus = order.PaymentStatus,
                Items = orderItems
            };

            ViewBag.OrderId = order.OrderId;
            return View(confirmation);
        }

        // GET: /Orders/MyTickets
        public async Task<IActionResult> MyTickets()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var tickets = await _context.Tickets
                .Include(t => t.Concert)
                    .ThenInclude(c => c!.Venue)
                .Include(t => t.Order)
                .Where(t => t.Order!.UserId == userId)
                .OrderByDescending(t => t.PurchaseDate)
                .ToListAsync();

            return View(tickets);
        }

        // GET: /Orders/TicketDetails/5
        public async Task<IActionResult> TicketDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var ticket = await _context.Tickets
                .Include(t => t.Concert)
                    .ThenInclude(c => c!.Venue)
                .Include(t => t.Order)
                .FirstOrDefaultAsync(t => t.TicketId == id && t.Order!.UserId == userId);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // Helper methods
        private string GenerateOrderNumber()
        {
            return $"JTX{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }

        private string GenerateTicketNumber()
        {
            return $"TKT{DateTime.Now:yyyyMMdd}{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        }

        private string GenerateQRCode(string orderNumber, int ticketIndex)
        {
            // In production, use a proper QR code library
            return $"QR-{orderNumber}-{ticketIndex:D3}";
        }

        private async Task ProcessPayment(Order order, string paymentMethod)
        {
            // Simulate payment processing
            // In production, integrate with actual payment gateways:
            // - FPX (Malaysian bank transfer)
            // - iPay88, MOLPay, or Billplz for Malaysia
            // - Credit/Debit card processors
            // - eWallets (TNG, GrabPay, Boost)

            await Task.Delay(100); // Simulate API call

            // For demo purposes, mark as completed
            order.PaymentStatus = "Completed";
            order.PaymentDate = DateTime.Now;
            order.TransactionId = $"TXN{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(10000, 99999)}";

            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
