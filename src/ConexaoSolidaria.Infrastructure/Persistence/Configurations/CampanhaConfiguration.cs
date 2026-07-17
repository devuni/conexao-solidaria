using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConexaoSolidaria.Infrastructure.Persistence.Configurations;

public sealed class CampanhaConfiguration : IEntityTypeConfiguration<Campanha>
{
    public void Configure(EntityTypeBuilder<Campanha> builder)
    {
        builder.ToTable("campanhas");

        builder.HasKey(campanha => campanha.Id);

        builder.Property(campanha => campanha.Id)
            .HasColumnName("id");

        builder.Property(campanha => campanha.Titulo)
            .HasColumnName("titulo")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(campanha => campanha.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(campanha => campanha.DataInicio)
            .HasColumnName("data_inicio")
            .IsRequired();

        builder.Property(campanha => campanha.DataFim)
            .HasColumnName("data_fim")
            .IsRequired();

        builder.Property(campanha => campanha.MetaFinanceira)
            .HasColumnName("meta_financeira")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(campanha => campanha.ValorTotalArrecadado)
            .HasColumnName("valor_total_arrecadado")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(campanha => campanha.Status)
            .HasColumnName("status")
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<StatusCampanha>(value))
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(campanha => campanha.CriadaEm)
            .HasColumnName("criada_em")
            .IsRequired();

        builder.Property(campanha => campanha.AtualizadaEm)
            .HasColumnName("atualizada_em");
    }
}
