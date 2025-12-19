using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WalletMate.Application.Models.Auth;
using WalletMate.Application.Ports.Out;

namespace WalletMate.Adapters.In.API.Auth;

public class TokenService(IConfiguration configuration) : ITokenServicePort
{
    public string GenerateToken(AuthTokenPayload user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = configuration["Jwt:Secret"];

        var key = Encoding.ASCII.GetBytes(secret ?? string.Empty);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            ]),
            Expires = DateTime.UtcNow.AddHours(double.Parse(configuration["Jwt:ExpiresInHours"] ?? "1")),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}