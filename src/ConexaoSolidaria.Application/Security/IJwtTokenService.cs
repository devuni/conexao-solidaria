using ConexaoSolidaria.Domain.Entities;

namespace ConexaoSolidaria.Application.Security;

public interface IJwtTokenService
{
    string GenerateToken(Usuario usuario);
}
