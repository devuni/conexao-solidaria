using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConexaoSolidaria.DonationWorker.Mongo;

public sealed class DoacaoProcessadaAuditDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    public Guid EventoId { get; set; }

    public Guid DoacaoId { get; set; }

    public Guid CampanhaId { get; set; }

    public Guid DoadorId { get; set; }

    public decimal ValorDoacao { get; set; }

    public DateTimeOffset RecebidaEm { get; set; }

    public DateTimeOffset ProcessadaEm { get; set; }
}
