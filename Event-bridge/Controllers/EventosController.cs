using Event_bridge.Data;
using Event_bridge.Interfaces;
using Event_bridge.Model;
using Event_bridge.Model.DTOs.Eventos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Event_bridge.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventosController : ControllerBase
{
    public readonly AppDbContext _Db;
    public readonly ITokenServices _Token;
    public EventosController(AppDbContext db, ITokenServices token)
    {
        _Db = db;
        _Token = token;
    }

    [Authorize]
    [HttpPost("Criar")]
    public async Task<IActionResult> Criar([FromBody] CriarEventosDto dto)
    {
        var usuarioEmail = User.FindFirst(ClaimTypes.Email)!.Value;
        var usuario = _Db.Usuarios.FirstOrDefault(u => u.Email == usuarioEmail);
        var evento = new EventosModel
        {
            IdUsuario = usuario.Id,
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim
        };

        _Db.Eventos.Add(evento);
        await _Db.SaveChangesAsync();
        return Ok("Evento criado com sucesso.");
    }
    [Authorize]
    [HttpGet("Eventos")]
    public IActionResult GetEventos()
    {

    }
    [Authorize]
    [HttpPost("Remarcar")]
    public IActionResult RemarcarEvento([FromBody] RemarcaEventosDto dto)
    {
        var usuarioEmail = User.FindFirst(ClaimTypes.Email)!.Value;
        var usuario = _Db.Usuarios.FirstOrDefault(u => u.Email == usuarioEmail);
        
           
    }
}

