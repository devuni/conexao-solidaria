namespace ConexaoSolidaria.Application.Security;

public interface ICpfValidator
{
    bool IsValid(string cpf);

    string OnlyDigits(string cpf);
}
