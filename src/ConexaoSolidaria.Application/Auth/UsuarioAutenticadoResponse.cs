namespace ConexaoSolidaria.Application.Auth;

public sealed record UsuarioAutenticadoResponse(
    Guid Id,
    string NomeCompleto,
    string Email,
    string Perfil);
