using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using JTX_Ecom.Data;
using JTX_Ecom.Models;
using JTX_Ecom.Models.DTOs;

namespace JTX_Ecom.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<ProfileController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /Profile
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            var profileDto = new UserProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = profile?.PhoneNumber,
                Address = profile?.Address,
                City = profile?.City,
                State = profile?.State,
                PostalCode = profile?.PostalCode,
                DateOfBirth = profile?.DateOfBirth,
                ICNumber = profile?.ICNumber,
                ReceiveNewsletter = profile?.ReceiveNewsletter ?? true
            };

            ViewBag.Email = user.Email;
            ViewBag.MemberSince = user.CreatedAt.ToString("MMMM yyyy");
            ViewBag.MalaysianStates = GetMalaysianStates();

            return View(profileDto);
        }

        // POST: /Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MalaysianStates = GetMalaysianStates();
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Update user basic info
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                await _userManager.UpdateAsync(user);

                // Update or create profile
                var profile = await _context.UserProfiles
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

                if (profile == null)
                {
                    profile = new UserProfile
                    {
                        UserId = user.Id
                    };
                    _context.UserProfiles.Add(profile);
                }

                profile.PhoneNumber = model.PhoneNumber;
                profile.Address = model.Address;
                profile.City = model.City;
                profile.State = model.State;
                profile.PostalCode = model.PostalCode;
                profile.DateOfBirth = model.DateOfBirth;
                profile.ICNumber = model.ICNumber;
                profile.ReceiveNewsletter = model.ReceiveNewsletter;
                profile.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                TempData["ErrorMessage"] = "An error occurred while updating your profile";
                ViewBag.MalaysianStates = GetMalaysianStates();
                return View("Index", model);
            }
        }

        // GET: /Profile/ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Profile/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password changed successfully!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // GET: /Profile/Statistics
        public async Task<IActionResult> Statistics()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var stats = new
            {
                TotalOrders = await _context.Orders.CountAsync(o => o.UserId == userId),
                TotalTickets = await _context.Tickets
                    .Where(t => t.Order!.UserId == userId)
                    .CountAsync(),
                TotalSpent = await _context.Orders
                    .Where(o => o.UserId == userId && o.PaymentStatus == "Completed")
                    .SumAsync(o => o.TotalAmount),
                UpcomingEvents = await _context.Tickets
                    .Include(t => t.Concert)
                    .Where(t => t.Order!.UserId == userId && t.Concert!.EventDate > DateTime.Now)
                    .Select(t => t.Concert!.EventDate)
                    .Distinct()
                    .CountAsync()
            };

            ViewBag.Stats = stats;
            return View();
        }

        // Helper method
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
