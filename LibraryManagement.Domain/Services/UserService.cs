using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using LibraryManagement.Data.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserService(
        IUserRepository userRepository, 
        IConfiguration configuration,
        ILogger<UserService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<string?> LoginAsync(LoginVM request)
    {
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["UserId"] = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value ?? "Unknown",
            ["RoleName"] = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown"
        }))
        {
            _logger.LogInformation("Login attempt for UserName: {UserName}", request.UserName);
            var user = await _userRepository.GetByUsernameAsync(request.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                _logger.LogWarning("Invalid login attempt for UserName: {UserName}, user with this name doesn't exist or invalid Password", request.UserName);
                return null;
            }

            // Generate JWT token
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        };
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["TokenLifetimeMinutes"])),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("Login successful for UserName: {UserName}, UserId: {UserId}, Role: {RoleName}",
                    user.UserName, user.UserId, user.Role.RoleName);
            return tokenString;
        }
    }
}
