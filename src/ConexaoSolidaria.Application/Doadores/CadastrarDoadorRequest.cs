namespace ConexaoSolidaria.Application.Doadores;

public sealed record CadastrarDoadorRequest(
    string NomeCompleto,
    string Email,
    string Cpf,
    string Senha);
