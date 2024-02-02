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
        var usuario = _Token.GetUserByToken(User);
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
    [HttpGet("GetEventos")]
    public IActionResult GetEventos()
    {        
        var usuario = _Token.GetUserByToken(User);
        var eventosUsuario = _Db.Eventos.Where(e => e.IdUsuario == usuario.Id).ToList();

        return Ok(eventosUsuario);
    }

    [Authorize]
    [HttpPost("Editar")]
    public IActionResult EditarEvento([FromBody] EditarEventosDto dto)
    {
        var usuario = _Token.GetUserByToken(User);
        var evento = _Db.Eventos.FirstOrDefault(e => e.Id == dto.Id);

        if (evento == null || usuario.Id != evento.IdUsuario)
            return Unauthorized("Acesso negado.");  

        evento.DataInicio = dto.NovaDataInicio;
        evento.DataFim = dto.NovaDataFim;
        evento.Titulo = dto.Titulo;
        evento.Descricao = dto.Descricao;
        
        _Db.SaveChanges();
        return Ok("Evento editado com sucesso.");
    }

    [Authorize]
    [HttpDelete("Delete")]
    public IActionResult DeletarEvento([FromBody] DeletarEventosDto dto) 
    {
        var usuario = _Token.GetUserByToken(User);
        var evento = _Db.Eventos.FirstOrDefault(e => e.Id == dto.IdUsuario);

        if (evento == null || usuario.Id != evento.IdUsuario)
            return Unauthorized("Acesso negado.");

        _Db.Remove(evento);
        _Db.SaveChanges();

        return Ok("Evento Deletado Com sucesso.");
    }
}

