namespace ConexaoSolidaria.Contracts;

public sealed record ServiceStatusResponse(
    string Service,
    string Status,
    DateTimeOffset Timestamp);
