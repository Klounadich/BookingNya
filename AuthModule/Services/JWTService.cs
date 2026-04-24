using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthModule.Commands;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthModule.Services;

public class JWTService
{
    public class AuthSettings
    {
        public TimeSpan Expires { get; set; }
        
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
    
    private readonly AuthSettings _options;

    public JWTService(IOptions<AuthSettings> options)
    {
        _options = options.Value;
    }
    public async Task<string> GenerateToken(JWTRequestCommand request )
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, request.display_name),
            new Claim(ClaimTypes.Email, request.email),
            new Claim(ClaimTypes.NameIdentifier, request.id.ToString())
                
        };           
        
        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(_options.Expires),
            claims: claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}