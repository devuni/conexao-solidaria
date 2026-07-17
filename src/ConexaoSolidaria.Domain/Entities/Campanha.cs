using ConexaoSolidaria.Domain.Common;
using ConexaoSolidaria.Domain.Enums;

namespace ConexaoSolidaria.Domain.Entities;

public sealed class Campanha
{
    private Campanha()
    {
    }

    public Campanha(
        string titulo,
        string descricao,
        DateTimeOffset dataInicio,
        DateTimeOffset dataFim,
        decimal metaFinanceira,
        StatusCampanha status)
    {
        ValidarDados(titulo, descricao, dataInicio, dataFim, metaFinanceira);

        Id = Guid.NewGuid();
        Titulo = titulo.Trim();
        Descricao = descricao.Trim();
        DataInicio = dataInicio.ToUniversalTime();
        DataFim = dataFim.ToUniversalTime();
        MetaFinanceira = metaFinanceira;
        ValorTotalArrecadado = 0m;
        Status = status;
        CriadaEm = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }

    public string Titulo { get; private set; } = null!;

    public string Descricao { get; private set; } = null!;

    public DateTimeOffset DataInicio { get; private set; }

    public DateTimeOffset DataFim { get; private set; }

    public decimal MetaFinanceira { get; private set; }

    public decimal ValorTotalArrecadado { get; private set; }

    public StatusCampanha Status { get; private set; }

    public DateTimeOffset CriadaEm { get; private set; }

    public DateTimeOffset? AtualizadaEm { get; private set; }

    public void Atualizar(
        string titulo,
        string descricao,
        DateTimeOffset dataInicio,
        DateTimeOffset dataFim,
        decimal metaFinanceira,
        StatusCampanha status)
    {
        ValidarDados(titulo, descricao, dataInicio, dataFim, metaFinanceira);

        Titulo = titulo.Trim();
        Descricao = descricao.Trim();
        DataInicio = dataInicio.ToUniversalTime();
        DataFim = dataFim.ToUniversalTime();
        MetaFinanceira = metaFinanceira;
        Status = status;
        AtualizadaEm = DateTimeOffset.UtcNow;
    }

    public void ValidarPodeReceberDoacao()
    {
        if (Status is StatusCampanha.Concluida or StatusCampanha.Cancelada)
            throw new DomainException("Não é permitido doar para campanhas concluídas ou canceladas.");

        if (DataFim < DateTimeOffset.UtcNow)
            throw new DomainException("Não é permitido doar para campanhas encerradas.");
    }

    public void RegistrarDoacao(decimal valor)
    {
        ValidarPodeReceberDoacao();

        if (valor <= 0)
            throw new DomainException("O valor da doação deve ser maior que zero.");

        ValorTotalArrecadado += valor;
        AtualizadaEm = DateTimeOffset.UtcNow;
    }

    private static void ValidarDados(
        string titulo,
        string descricao,
        DateTimeOffset dataInicio,
        DateTimeOffset dataFim,
        decimal metaFinanceira)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("O título da campanha é obrigatório.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("A descrição da campanha é obrigatória.");

        if (dataFim < DateTimeOffset.UtcNow)
            throw new DomainException("A campanha não pode ser criada ou editada com data de término no passado.");

        if (dataFim <= dataInicio)
            throw new DomainException("A data de término deve ser posterior à data de início.");

        if (metaFinanceira <= 0)
            throw new DomainException("A meta financeira deve ser maior que zero.");
    }
}
