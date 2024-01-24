namespace Event_bridge.Model;

public class UsuariosModel
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public string Senha { get; set; }

    public DateTime CriadoEm { get; set; }

    public DateTime AltualizadoEm { get; set; }
}
