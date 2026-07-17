namespace ConexaoSolidaria.Application.Security;

public interface IPasswordHashService
{
    string Hash(string password);

    bool Verify(string password, string passwordHash);
}
