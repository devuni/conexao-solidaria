using ConexaoSolidaria.Domain.Common;
using ConexaoSolidaria.Domain.Enums;

namespace ConexaoSolidaria.Domain.Entities;

public sealed class Doacao
{
    private Doacao()
    {
    }

    public Doacao(
        Guid campanhaId,
        Guid doadorId,
        decimal valor)
    {
        if (campanhaId == Guid.Empty)
            throw new DomainException("A campanha da doação é obrigatória.");

        if (doadorId == Guid.Empty)
            throw new DomainException("O doador da doação é obrigatório.");

        if (valor <= 0)
            throw new DomainException("O valor da doação deve ser maior que zero.");

        Id = Guid.NewGuid();
        CampanhaId = campanhaId;
        DoadorId = doadorId;
        Valor = valor;
        Status = StatusDoacao.Recebida;
        CriadaEm = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid CampanhaId { get; private set; }

    public Guid DoadorId { get; private set; }

    public decimal Valor { get; private set; }

    public StatusDoacao Status { get; private set; }

    public DateTimeOffset CriadaEm { get; private set; }

    public DateTimeOffset? ProcessadaEm { get; private set; }

    public void MarcarComoProcessada()
    {
        if (Status == StatusDoacao.Processada)
            return;

        Status = StatusDoacao.Processada;
        ProcessadaEm = DateTimeOffset.UtcNow;
    }

    public void MarcarComoFalha()
    {
        Status = StatusDoacao.Falha;
    }
}
