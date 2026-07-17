namespace ConexaoSolidaria.Application.Doacoes;

public sealed record RegistrarDoacaoResponse(
    Guid IdDoacao,
    string Status,
    string Mensagem);
