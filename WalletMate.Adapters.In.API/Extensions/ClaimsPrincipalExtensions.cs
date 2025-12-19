using System.Security.Claims;

namespace WalletMate.Adapters.In.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(id, out var guid) ? guid : throw new UnauthorizedAccessException("User is not authorized");
    }
}