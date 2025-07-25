using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SupplierManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Get JWT token for authentication
        /// </summary>
        /// <param name="loginRequest">Login credentials</param>
        /// <returns>JWT token</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                // Simple authentication - in production, validate against database/identity provider
                if (IsValidUser(loginRequest.Username, loginRequest.Password))
                {
                    var token = GenerateJwtToken(loginRequest.Username);
                    _logger.LogInformation("User {Username} successfully authenticated", loginRequest.Username);

                    return Ok(new {
                        token = token,
                        expires = DateTime.UtcNow.AddHours(1),
                        username = loginRequest.Username
                    });
                }

                _logger.LogWarning("Failed authentication attempt for user {Username}", loginRequest.Username);
                return Unauthorized(new { message = "Invalid username or password" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during authentication for user {Username}", loginRequest.Username);
                return StatusCode(500, "Internal server error occurred");
            }
        }

        private bool IsValidUser(string username, string password)
        {
            // Simple hardcoded validation for demo purposes
            // In production, validate against database, Active Directory, etc.
            var validUsers = new Dictionary<string, string>
            {
                { "admin", "password123" },
                { "user", "user123" },
                { "demo", "demo123" }
            };

            return validUsers.ContainsKey(username) && validUsers[username] == password;
        }

        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "your-secret-key-here-must-be-at-least-32-characters"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim("username", username)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "SupplierManagement.API",
                audience: _configuration["Jwt:Audience"] ?? "SupplierManagement.API",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
