namespace ConexaoSolidaria.Application.Campanhas;

public sealed record CriarCampanhaRequest(
    string Titulo,
    string Descricao,
    DateTimeOffset DataInicio,
    DateTimeOffset DataFim,
    decimal MetaFinanceira,
    string Status);
