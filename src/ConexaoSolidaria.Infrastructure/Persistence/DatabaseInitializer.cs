using ConexaoSolidaria.Application.Security;
using ConexaoSolidaria.Domain.Entities;
using ConexaoSolidaria.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConexaoSolidaria.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordHashService = scope.ServiceProvider.GetRequiredService<IPasswordHashService>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        await context.Database.EnsureCreatedAsync();

        var gestorEmail = configuration["Seed:Gestor:Email"] ?? "gestor@conexaosolidaria.org";
        var gestorSenha = configuration["Seed:Gestor:Senha"] ?? "Gestor@123456";
        var gestorNome = configuration["Seed:Gestor:Nome"] ?? "Gestor ONG";
        var gestorCpf = configuration["Seed:Gestor:Cpf"] ?? "93541134780";

        var exists = await context.Usuarios.AnyAsync(
            usuario => usuario.Email == gestorEmail);

        if (exists)
        {
            logger.LogInformation("Usuário gestor inicial já existe.");
            return;
        }

        var gestor = new Usuario(
            gestorNome,
            gestorEmail,
            gestorCpf,
            passwordHashService.Hash(gestorSenha),
            PerfilUsuario.GestorONG);

        context.Usuarios.Add(gestor);
        await context.SaveChangesAsync();

        logger.LogInformation("Usuário gestor inicial criado: {Email}", gestorEmail);
    }
}
