using ConexaoSolidaria.Application.Auth;
using ConexaoSolidaria.Application.Common;
using ConexaoSolidaria.Application.Security;
using ConexaoSolidaria.Infrastructure.Persistence;
using ConexaoSolidaria.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ConexaoSolidaria.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController(
    ApplicationDbContext context,
    IPasswordHashService passwordHashService,
    IJwtTokenService jwtTokenService,
    IOptions<JwtOptions> jwtOptions) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiErrorResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> LoginAsync(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
        {
            return BadRequest(new ApiErrorResponse(
                "LOGIN_INVALIDO",
                "E-mail e senha são obrigatórios."));
        }

        var email = request.Email.Trim().ToLowerInvariant();

        var usuario = await context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (usuario is null || !passwordHashService.Verify(request.Senha, usuario.SenhaHash))
        {
            return Unauthorized(new ApiErrorResponse(
                "CREDENCIAIS_INVALIDAS",
                "E-mail ou senha inválidos."));
        }

        var token = jwtTokenService.GenerateToken(usuario);

        var response = new LoginResponse(
            token,
            "Bearer",
            jwtOptions.Value.ExpiresInMinutes * 60,
            new UsuarioAutenticadoResponse(
                usuario.Id,
                usuario.NomeCompleto,
                usuario.Email,
                usuario.Perfil.ToString()));

        return Ok(response);
    }
}
