using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConexaoSolidaria.Infrastructure.Persistence.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(usuario => usuario.Id);

        builder.Property(usuario => usuario.Id)
            .HasColumnName("id");

        builder.Property(usuario => usuario.NomeCompleto)
            .HasColumnName("nome_completo")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(usuario => usuario.Email)
            .HasColumnName("email")
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(usuario => usuario.Email)
            .IsUnique();

        builder.Property(usuario => usuario.Cpf)
            .HasColumnName("cpf")
            .HasMaxLength(11)
            .IsRequired();

        builder.HasIndex(usuario => usuario.Cpf)
            .IsUnique();

        builder.Property(usuario => usuario.SenhaHash)
            .HasColumnName("senha_hash")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(usuario => usuario.Perfil)
            .HasColumnName("perfil")
            .HasConversion(
                perfil => perfil.ToString(),
                value => Enum.Parse<PerfilUsuario>(value))
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(usuario => usuario.CriadoEm)
            .HasColumnName("criado_em")
            .IsRequired();
    }
}
