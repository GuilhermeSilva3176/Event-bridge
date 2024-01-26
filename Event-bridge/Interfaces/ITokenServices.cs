using Event_bridge.Model;
using System.Security.Claims;

namespace Event_bridge.Interfaces;

public interface ITokenServices
{
    UsuariosModel GetUserByToken(ClaimsPrincipal user);
    string GerarToken(UsuariosModel usuario);
}
