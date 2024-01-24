using Event_bridge.Data;
using Event_bridge.Interfaces;
using Event_bridge.Model;
using Event_bridge.Model.DTOs.Usuario;
using Event_bridge.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_bridge.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _Db;
    private readonly IPasswordServices _PasswordService;
    public UsuariosController(AppDbContext db, IPasswordServices passwordService) 
    {
        _Db = db;
        _PasswordService = passwordService;
    }

    [HttpPost("Registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarDto dto)
    {
        if(_Db.Usuarios.Any(u => u.Email == dto.Email))
          return BadRequest("Este email já foi registrado.");

        var usuario = new UsuariosModel
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = _PasswordService.GerarHashSenha(dto.Senha),
            CriadoEm = DateTime.Now,
            AltualizadoEm = DateTime.Now,
        };

        _Db.Usuarios.Add(usuario);
        await _Db.SaveChangesAsync();

        return Ok("Usuário registrado com sucesso!");
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var usuario = _Db.Usuarios.FirstOrDefault(u => u.Email == dto.Email);

        if (usuario == null || !_PasswordService.VerificarSenha(dto.Senha, usuario.Senha))
            return BadRequest("Credênciais inválidas.");

        return Ok("Login realizado com sucesso!");
    }

    [HttpDelete("Delete")]
    public IActionResult Deletar([FromBody] DeletarDto dto)
    {
        var usuario = _Db.Usuarios.FirstOrDefault(u => u.Email == dto.Email);

        if (usuario == null || usuario.Senha != dto.Senha)
            return BadRequest("Credênciais inválidas");
        
        _Db.Usuarios.Remove(usuario);
        _Db.SaveChanges();

        return Ok("Conta deletada com sucesso.");
    }
}
