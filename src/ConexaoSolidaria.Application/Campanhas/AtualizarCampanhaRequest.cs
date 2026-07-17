namespace ConexaoSolidaria.Application.Campanhas;

public sealed record AtualizarCampanhaRequest(
    string Titulo,
    string Descricao,
    DateTimeOffset DataInicio,
    DateTimeOffset DataFim,
    decimal MetaFinanceira,
    string Status);
