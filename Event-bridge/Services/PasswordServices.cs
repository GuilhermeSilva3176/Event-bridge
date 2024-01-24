using Event_bridge.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Event_bridge.Services;

public class PasswordServices : IPasswordServices
{
    public string GerarHashSenha(string senha)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }      
    }

    public bool VerificarSenha(string senhaDigitada, string hashArmazenado)
    {
        string hashSenha = GerarHashSenha(senhaDigitada);
        return hashSenha == hashArmazenado;
    }
}
