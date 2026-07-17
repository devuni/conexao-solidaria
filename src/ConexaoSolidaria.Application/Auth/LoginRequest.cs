namespace ConexaoSolidaria.Application.Auth;

public sealed record LoginRequest(
    string Email,
    string Senha);
