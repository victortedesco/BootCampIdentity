using BootCamp24.Auth.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BootCamp24.Auth.API.Services;

public class JWTService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _jwtKey;

    public JWTService(IConfiguration configuration)
    {
        _configuration = configuration;
        _jwtKey = new(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
    }

    public string JWTGenerateToken(User user)
    {
        var userClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new("MyCustomClaim", "MyCustomValue")
        };
        var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(userClaims),
            Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:ExpiresInDays"])),
            SigningCredentials = credentials,
            Issuer = _configuration["JWT:Issuer"],
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(jwt);
    }
}
