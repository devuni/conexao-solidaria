using ConexaoSolidaria.Application.Abstractions;
using ConexaoSolidaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Campanha> Campanhas => Set<Campanha>();

    public DbSet<Doacao> Doacoes => Set<Doacao>();

    public DbSet<EventoProcessado> EventosProcessados => Set<EventoProcessado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
