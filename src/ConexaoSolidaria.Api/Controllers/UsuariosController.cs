using ConexaoSolidaria.Application.Auth;
using ConexaoSolidaria.Application.Common;
using ConexaoSolidaria.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Api.Controllers;

[ApiController]
[Route("api/v1/usuarios")]
public sealed class UsuariosController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType<UsuarioAutenticadoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UsuarioAutenticadoResponse>> ObterUsuarioLogadoAsync(
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new ApiErrorResponse(
                "TOKEN_INVALIDO",
                "Token inválido."));
        }

        var usuario = await context.Usuarios
            .AsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => new UsuarioAutenticadoResponse(
                x.Id,
                x.NomeCompleto,
                x.Email,
                x.Perfil.ToString()))
            .FirstOrDefaultAsync(cancellationToken);

        if (usuario is null)
        {
            return Unauthorized(new ApiErrorResponse(
                "USUARIO_NAO_ENCONTRADO",
                "Usuário não encontrado."));
        }

        return Ok(usuario);
    }

    [HttpGet("gestor/check")]
    [Authorize(Roles = "GestorONG")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult CheckGestor()
    {
        return NoContent();
    }

    [HttpGet("doador/check")]
    [Authorize(Roles = "Doador")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult CheckDoador()
    {
        return NoContent();
    }
}
