using Event_bridge.Data;
using Event_bridge.Interfaces;
using Event_bridge.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Event_bridge.Services;

public class TokenServices : ITokenServices
{
    private readonly IConfiguration _Configuration;
    private readonly AppDbContext _Db;

    public TokenServices(IConfiguration configuration, AppDbContext db)
    {
        _Configuration = configuration;
        _Db = db;
    }

    public string GerarToken(UsuariosModel usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_Configuration["JwtConfig:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Email, usuario.Email),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public UsuariosModel GetUserByToken(ClaimsPrincipal user)
    {
        var usuarioEmail = user.FindFirst(ClaimTypes.Email)!.Value;
        return _Db.Usuarios.FirstOrDefault(u => u.Email == usuarioEmail)!;        
    }
}
