using Event_bridge.Model;

namespace Event_bridge.Interfaces;

public interface ITokenServices
{
    string GerarToken(UsuariosModel usuario);
}
