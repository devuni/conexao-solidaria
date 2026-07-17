using ConexaoSolidaria.Domain.Common;

namespace ConexaoSolidaria.Domain.Entities;

public sealed class EventoProcessado
{
    private EventoProcessado()
    {
    }

    public EventoProcessado(Guid eventoId, Guid doacaoId)
    {
        if (eventoId == Guid.Empty)
            throw new DomainException("O identificador do evento é obrigatório.");

        if (doacaoId == Guid.Empty)
            throw new DomainException("O identificador da doação é obrigatório.");

        EventoId = eventoId;
        DoacaoId = doacaoId;
        ProcessadoEm = DateTimeOffset.UtcNow;
    }

    public Guid EventoId { get; private set; }

    public Guid DoacaoId { get; private set; }

    public DateTimeOffset ProcessadoEm { get; private set; }
}
