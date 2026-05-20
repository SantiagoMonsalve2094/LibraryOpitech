using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryOpitech.Application.DTOs.Auth;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryOpitech.Infrastructure.Authentication;

public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    public AuthResponse Generate(User user)
    {
        var secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt secret is not configured.");
        var issuer = configuration["Jwt:Issuer"] ?? "LibraryOpitech";
        var audience = configuration["Jwt:Audience"] ?? "LibraryOpitech";
        var expirationHours = int.TryParse(configuration["Jwt:ExpirationHours"], out var hours) ? hours : 8;
        var expiresAt = DateTime.UtcNow.AddHours(expirationHours);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims, expires: expiresAt, signingCredentials: credentials);

        return new AuthResponse
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt
        };
    }
}
