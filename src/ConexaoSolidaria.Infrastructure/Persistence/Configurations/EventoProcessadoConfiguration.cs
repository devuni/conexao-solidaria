using ConexaoSolidaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConexaoSolidaria.Infrastructure.Persistence.Configurations;

public sealed class EventoProcessadoConfiguration : IEntityTypeConfiguration<EventoProcessado>
{
    public void Configure(EntityTypeBuilder<EventoProcessado> builder)
    {
        builder.ToTable("eventos_processados");

        builder.HasKey(evento => evento.EventoId);

        builder.Property(evento => evento.EventoId)
            .HasColumnName("evento_id");

        builder.Property(evento => evento.DoacaoId)
            .HasColumnName("doacao_id")
            .IsRequired();

        builder.HasIndex(evento => evento.DoacaoId);

        builder.Property(evento => evento.ProcessadoEm)
            .HasColumnName("processado_em")
            .IsRequired();
    }
}
