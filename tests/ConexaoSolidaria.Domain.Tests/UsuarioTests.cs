using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.Domain.Enums;

namespace ConexaoSolidaria.Domain.Tests;

public sealed class UsuarioTests
{
    [Fact]
    public void Deve_criar_usuario_doador_com_email_normalizado()
    {
        var usuario = new Usuario(
            "Maria Doadora",
            "MARIA@EMAIL.COM",
            "52998224725",
            "hash-da-senha",
            PerfilUsuario.Doador);

        Assert.Equal("maria@email.com", usuario.Email);
        Assert.Equal(PerfilUsuario.Doador, usuario.Perfil);
    }
}
