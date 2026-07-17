namespace ConexaoSolidaria.Application.Campanhas;

public sealed record TransparenciaCampanhaResponse(
    Guid Id,
    string Titulo,
    decimal MetaFinanceira,
    decimal ValorTotalArrecadado);
