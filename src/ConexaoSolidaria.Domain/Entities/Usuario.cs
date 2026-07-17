using ConexaoSolidaria.Domain.Common;
using ConexaoSolidaria.Domain.Enums;

namespace ConexaoSolidaria.Domain.Entities;

public sealed class Usuario
{
    private Usuario()
    {
    }

    public Usuario(
        string nomeCompleto,
        string email,
        string cpf,
        string senhaHash,
        PerfilUsuario perfil)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
            throw new DomainException("O nome completo é obrigatório.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("O e-mail é obrigatório.");

        if (string.IsNullOrWhiteSpace(cpf))
            throw new DomainException("O CPF é obrigatório.");

        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new DomainException("A senha é obrigatória.");

        Id = Guid.NewGuid();
        NomeCompleto = nomeCompleto.Trim();
        Email = email.Trim().ToLowerInvariant();
        Cpf = cpf;
        SenhaHash = senhaHash;
        Perfil = perfil;
        CriadoEm = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }

    public string NomeCompleto { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string Cpf { get; private set; } = null!;

    public string SenhaHash { get; private set; } = null!;

    public PerfilUsuario Perfil { get; private set; }

    public DateTimeOffset CriadoEm { get; private set; }
}
