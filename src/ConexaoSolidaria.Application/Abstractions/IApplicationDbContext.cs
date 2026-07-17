using ConexaoSolidaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Usuario> Usuarios { get; }

    DbSet<Campanha> Campanhas { get; }

    DbSet<Doacao> Doacoes { get; }

    DbSet<EventoProcessado> EventosProcessados { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
