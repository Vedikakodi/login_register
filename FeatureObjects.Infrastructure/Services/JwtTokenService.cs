using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DataStore.Abstraction.Models;

namespace FeatureObjects.Infrastructure.Services
{
    public class JwtTokenService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtTokenService(IConfiguration config)
        {
            _key = config["Jwt:Key"] ?? throw new ArgumentNullException(nameof(config), "JWT Key is missing");
            _issuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException(nameof(config), "JWT Issuer is missing");
            _audience = config["Jwt:Audience"] ?? throw new ArgumentNullException(nameof(config), "JWT Audience is missing");
        }

        // ✅ Ensure this method exists
        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
