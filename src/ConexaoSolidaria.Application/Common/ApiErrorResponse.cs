namespace ConexaoSolidaria.Application.Common;

public sealed record ApiErrorResponse(
    string Code,
    string Message);
