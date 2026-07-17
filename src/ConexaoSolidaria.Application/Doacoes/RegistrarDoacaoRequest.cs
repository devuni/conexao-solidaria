namespace ConexaoSolidaria.Application.Doacoes;

public sealed record RegistrarDoacaoRequest(
    Guid IdCampanha,
    decimal ValorDoacao);
