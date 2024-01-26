using Event_bridge.Data;
using Event_bridge.Interfaces;
using Event_bridge.Model;
using Event_bridge.Model.DTOs.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Event_bridge.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _Db;
    private readonly IPasswordServices _PasswordService;
    private readonly ITokenServices _Token;
    public UsuariosController(AppDbContext db, IPasswordServices passwordService,
        ITokenServices token)
    {
        _Db = db;
        _PasswordService = passwordService;
        _Token = token;
    }

    [HttpPost("Registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarDto dto)
    {
        if (_Db.Usuarios.Any(u => u.Email == dto.Email))
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
            return BadRequest(new { mensagem = "Credênciais inválidas ou usuário não existe." });

        var token = _Token.GerarToken(usuario);
        return Ok(new { Token = token });
    }

    [Authorize]
    [HttpDelete("Delete")]
    public IActionResult Deletar([FromBody] DeletarDto dto)
    {
        var usuario = _Token.GetUserByToken(User);

        if (usuario == null || !_PasswordService.VerificarSenha(dto.Senha, usuario.Senha))
            return BadRequest(new { mensagem = "Credênciais inválidas ou usuário não existe." });

        _Db.Usuarios.Remove(usuario);
        _Db.SaveChanges();

        return Ok("Conta deletada com sucesso.");
    }

    [Authorize]
    [HttpPost("AlterarSenha")]
    public IActionResult AlterarSenha([FromBody] AlterarSenhaDto dto)
    {
        var usuario = _Token.GetUserByToken(User);

        if (!_PasswordService.VerificarSenha(dto.SenhaAtual, usuario.Senha))
            return BadRequest("Credênciais inválidas");

        usuario.Senha = _PasswordService.GerarHashSenha(dto.SenhaNova);
        usuario.AltualizadoEm = DateTime.Now;
        _Db.SaveChanges();

        return Ok("Senha alterada com sucesso.");
    }
}
