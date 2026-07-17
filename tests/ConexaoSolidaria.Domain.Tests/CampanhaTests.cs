using ConexaoSolidaria.Domain.Common;
using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.Domain.Enums;

namespace ConexaoSolidaria.Domain.Tests;

public sealed class CampanhaTests
{
    [Fact]
    public void Deve_criar_campanha_valida()
    {
        var campanha = new Campanha(
            "Natal Solidário",
            "Campanha para arrecadação de Natal.",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddDays(30),
            10000m,
            StatusCampanha.Ativa);

        Assert.Equal("Natal Solidário", campanha.Titulo);
        Assert.Equal(10000m, campanha.MetaFinanceira);
        Assert.Equal(0m, campanha.ValorTotalArrecadado);
        Assert.Equal(StatusCampanha.Ativa, campanha.Status);
    }

    [Fact]
    public void Nao_deve_criar_campanha_com_meta_zero()
    {
        Assert.Throws<DomainException>(() => new Campanha(
            "Natal Solidário",
            "Campanha para arrecadação de Natal.",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddDays(30),
            0m,
            StatusCampanha.Ativa));
    }

    [Fact]
    public void Nao_deve_criar_campanha_com_data_fim_no_passado()
    {
        Assert.Throws<DomainException>(() => new Campanha(
            "Natal Solidário",
            "Campanha para arrecadação de Natal.",
            DateTimeOffset.UtcNow.AddDays(-10),
            DateTimeOffset.UtcNow.AddDays(-1),
            10000m,
            StatusCampanha.Ativa));
    }

    [Fact]
    public void Deve_registrar_doacao_em_campanha_ativa()
    {
        var campanha = new Campanha(
            "Natal Solidário",
            "Campanha para arrecadação de Natal.",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddDays(30),
            10000m,
            StatusCampanha.Ativa);

        campanha.RegistrarDoacao(150m);

        Assert.Equal(150m, campanha.ValorTotalArrecadado);
    }

    [Fact]
    public void Nao_deve_registrar_doacao_em_campanha_cancelada()
    {
        var campanha = new Campanha(
            "Natal Solidário",
            "Campanha para arrecadação de Natal.",
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddDays(30),
            10000m,
            StatusCampanha.Cancelada);

        Assert.Throws<DomainException>(() => campanha.RegistrarDoacao(150m));
    }
}
