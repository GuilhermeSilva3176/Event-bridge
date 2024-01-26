namespace Event_bridge.Model.DTOs.Eventos;

public class EditarEventosDto
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public DateTime NovaDataInicio { get; set; }
    public DateTime NovaDataFim { get; set; }

}