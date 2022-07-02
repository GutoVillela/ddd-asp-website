using KadoshWebsite.Infrastructure;
using KadoshWebsite.Services.Interfaces;
using KadoshWebsite.ValueObjects;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KadoshWebsite.Services
{
    public class TokenService : ITokenService
    {
        private static IList<RefreshToken> _refreshTokens = new List<RefreshToken>();

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public string GenerateToken(string username, string role)
        {
            var claims = new Claim[]
            {
                new(ClaimTypes.Name, username),
                new(ClaimTypes.Role, role)
            };
            return GenerateToken(claims);
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    key: new SymmetricSecurityKey(key),
                    algorithm: SecurityAlgorithms.HmacSha256Signature
                    )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Token inválido");

            return principal;
        }

        public void SaveRefreshToken(RefreshToken refreshToken)
        {
            ArgumentNullException.ThrowIfNull(refreshToken, nameof(refreshToken));
            if (!refreshToken.IsValid)
                throw new ArgumentException("Invalid refresh token");
            
            _refreshTokens.Add(refreshToken);
        }

        public string? GetRefreshToken(string username)
        {
            return _refreshTokens.FirstOrDefault(x => x.Username == username)?.Token;
        }

        public void DeleteRefreshToken(RefreshToken refreshToken)
        {
            var item = _refreshTokens.FirstOrDefault(x => x.Username == refreshToken.Username && x.Token == refreshToken.Token);
            if(item is not null) _refreshTokens.Remove(item);
        }
    }
}
