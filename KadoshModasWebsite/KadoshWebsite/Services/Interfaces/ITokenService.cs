using KadoshWebsite.ValueObjects;
using System.Security.Claims;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string username, string role);
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        void SaveRefreshToken(RefreshToken refreshToken);
        string? GetRefreshToken(string username);
        void DeleteRefreshToken(RefreshToken refreshToken);

    }
}
