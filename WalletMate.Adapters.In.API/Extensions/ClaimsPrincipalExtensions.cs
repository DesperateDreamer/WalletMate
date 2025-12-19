using System.Security.Claims;

namespace WalletMate.Adapters.In.API.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="ClaimsPrincipal"/> class to simplify accessing claims data.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Retrieves the unique identifier (GUID) of the current user from the given <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the authenticated user.</param>
    /// <returns>The GUID of the user if it exists and is valid.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized or the identifier is invalid.</exception>
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(id, out var guid) ? guid : throw new UnauthorizedAccessException("User is not authorized");
    }
}