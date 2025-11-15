using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JTX_Ecom.Models;
using JTX_Ecom.Models.DTOs;
using JTX_Ecom.Services;

namespace JTX_Ecom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "User registration failed", errors = result.Errors });
            }

            // Assign default "User" role
            await _userManager.AddToRoleAsync(user, "User");

            _logger.LogInformation("New user registered: {Email}", user.Email);

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT token
            var token = _jwtService.GenerateToken(user.Id, user.Email!, roles.ToList());

            // Update last login
            user.LastLoginAt = DateTime.Now;
            await _userManager.UpdateAsync(user);

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Roles = roles.ToList(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            _logger.LogInformation("User logged in: {Email}", user.Email);

            return Ok(response);
        }

        [HttpPost("admin/register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto, [FromHeader] string adminSecret)
        {
            // Simple admin secret key validation (you should use a more secure method in production)
            var configuredSecret = "JTX-Admin-Secret-2025"; // Store this in appsettings
            if (adminSecret != configuredSecret)
            {
                return Unauthorized(new { message = "Invalid admin secret" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Admin registration failed", errors = result.Errors });
            }

            // Assign "Admin" role
            await _userManager.AddToRoleAsync(user, "Admin");
            await _userManager.AddToRoleAsync(user, "User");

            _logger.LogInformation("New admin registered: {Email}", user.Email);

            return Ok(new { message = "Admin registered successfully" });
        }
    }
}
