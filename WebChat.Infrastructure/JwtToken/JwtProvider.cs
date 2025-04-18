using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    public string? GenerateToken(User user)
    {
        List<Claim> claims = new List<Claim>(){
            new Claim("Id", user.Id.ToString()),
            new Claim("Username", user.Username)
        };

        var signingCred = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),SecurityAlgorithms.HmacSha256);
        var expireTime = DateTime.UtcNow.Add(options.Value.Expires);

        var JwtToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials : signingCred,
            expires: expireTime
        );

        return new JwtSecurityTokenHandler().WriteToken(JwtToken);
    }
}