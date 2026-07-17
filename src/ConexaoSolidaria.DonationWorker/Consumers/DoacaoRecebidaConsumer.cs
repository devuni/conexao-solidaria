using ConexaoSolidaria.Contracts;
using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.DonationWorker.Mongo;
using ConexaoSolidaria.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace ConexaoSolidaria.DonationWorker.Consumers;

public sealed class DoacaoRecebidaConsumer(
    ApplicationDbContext context,
    IMongoClient mongoClient,
    ILogger<DoacaoRecebidaConsumer> logger) : IConsumer<DoacaoRecebidaEvent>
{
    public async Task Consume(ConsumeContext<DoacaoRecebidaEvent> consumeContext)
    {
        var evento = consumeContext.Message;

        logger.LogInformation(
            "Processando evento de doação {EventoId} para a campanha {CampanhaId}.",
            evento.EventoId,
            evento.CampanhaId);

        var jaProcessado = await context.EventosProcessados
            .AnyAsync(x => x.EventoId == evento.EventoId, consumeContext.CancellationToken);

        if (jaProcessado)
        {
            logger.LogInformation(
                "Evento {EventoId} já foi processado. Mensagem ignorada por idempotência.",
                evento.EventoId);

            return;
        }

        var doacao = await context.Doacoes
            .FirstOrDefaultAsync(x => x.Id == evento.DoacaoId, consumeContext.CancellationToken);

        if (doacao is null)
            throw new InvalidOperationException($"Doação {evento.DoacaoId} não encontrada.");

        var campanha = await context.Campanhas
            .FirstOrDefaultAsync(x => x.Id == evento.CampanhaId, consumeContext.CancellationToken);

        if (campanha is null)
            throw new InvalidOperationException($"Campanha {evento.CampanhaId} não encontrada.");

        campanha.RegistrarDoacao(evento.ValorDoacao);
        doacao.MarcarComoProcessada();
        context.EventosProcessados.Add(new EventoProcessado(evento.EventoId, evento.DoacaoId));

        await context.SaveChangesAsync(consumeContext.CancellationToken);

        await RegistrarAuditoriaMongoAsync(evento, consumeContext.CancellationToken);

        logger.LogInformation(
            "Evento de doação {EventoId} processado com sucesso. Doação {DoacaoId}. Valor {ValorDoacao}.",
            evento.EventoId,
            evento.DoacaoId,
            evento.ValorDoacao);
    }

    private async Task RegistrarAuditoriaMongoAsync(
        DoacaoRecebidaEvent evento,
        CancellationToken cancellationToken)
    {
        try
        {
            var database = mongoClient.GetDatabase("conexao_solidaria");
            var collection = database.GetCollection<DoacaoProcessadaAuditDocument>("doacoes_processadas_audit");

            var document = new DoacaoProcessadaAuditDocument
            {
                EventoId = evento.EventoId,
                DoacaoId = evento.DoacaoId,
                CampanhaId = evento.CampanhaId,
                DoadorId = evento.DoadorId,
                ValorDoacao = evento.ValorDoacao,
                RecebidaEm = evento.RecebidaEm,
                ProcessadaEm = DateTimeOffset.UtcNow
            };

            await collection.InsertOneAsync(document, cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogWarning(
                exception,
                "Não foi possível registrar auditoria MongoDB para o evento {EventoId}.",
                evento.EventoId);
        }
    }
}
