namespace ConexaoSolidaria.Contracts;

public sealed record DoacaoRecebidaEvent(
    Guid EventoId,
    Guid DoacaoId,
    Guid CampanhaId,
    Guid DoadorId,
    decimal ValorDoacao,
    DateTimeOffset RecebidaEm);
