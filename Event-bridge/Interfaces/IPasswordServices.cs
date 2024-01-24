namespace Event_bridge.Interfaces;

public interface IPasswordServices
{
    string GerarHashSenha(string senha);

    bool VerificarSenha(string senhaDigitada, string hashArmazenado);
}
