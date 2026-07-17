using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConexaoSolidaria.Infrastructure.Persistence.Configurations;

public sealed class DoacaoConfiguration : IEntityTypeConfiguration<Doacao>
{
    public void Configure(EntityTypeBuilder<Doacao> builder)
    {
        builder.ToTable("doacoes");

        builder.HasKey(doacao => doacao.Id);

        builder.Property(doacao => doacao.Id)
            .HasColumnName("id");

        builder.Property(doacao => doacao.CampanhaId)
            .HasColumnName("campanha_id")
            .IsRequired();

        builder.HasIndex(doacao => doacao.CampanhaId);

        builder.Property(doacao => doacao.DoadorId)
            .HasColumnName("doador_id")
            .IsRequired();

        builder.HasIndex(doacao => doacao.DoadorId);

        builder.Property(doacao => doacao.Valor)
            .HasColumnName("valor")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(doacao => doacao.Status)
            .HasColumnName("status")
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<StatusDoacao>(value))
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(doacao => doacao.CriadaEm)
            .HasColumnName("criada_em")
            .IsRequired();

        builder.Property(doacao => doacao.ProcessadaEm)
            .HasColumnName("processada_em");
    }
}
