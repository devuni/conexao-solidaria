using ConexaoSolidaria.Domain.Common;
using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.Domain.Enums;

namespace ConexaoSolidaria.Domain.Tests;

public sealed class DoacaoTests
{
    [Fact]
    public void Deve_criar_doacao_com_status_recebida()
    {
        var doacao = new Doacao(Guid.NewGuid(), Guid.NewGuid(), 150m);

        Assert.Equal(StatusDoacao.Recebida, doacao.Status);
        Assert.Equal(150m, doacao.Valor);
    }

    [Fact]
    public void Nao_deve_criar_doacao_com_valor_zero()
    {
        Assert.Throws<DomainException>(() =>
            new Doacao(Guid.NewGuid(), Guid.NewGuid(), 0m));
    }

    [Fact]
    public void Deve_marcar_doacao_como_processada()
    {
        var doacao = new Doacao(Guid.NewGuid(), Guid.NewGuid(), 100m);

        doacao.MarcarComoProcessada();

        Assert.Equal(StatusDoacao.Processada, doacao.Status);
        Assert.NotNull(doacao.ProcessadaEm);
    }
}
