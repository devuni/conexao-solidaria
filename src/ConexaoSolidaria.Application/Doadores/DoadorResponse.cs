namespace ConexaoSolidaria.Application.Doadores;

public sealed record DoadorResponse(
    Guid Id,
    string NomeCompleto,
    string Email,
    string Cpf,
    string Perfil);
