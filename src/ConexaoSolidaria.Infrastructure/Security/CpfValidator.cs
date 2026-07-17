using ConexaoSolidaria.Application.Security;

namespace ConexaoSolidaria.Infrastructure.Security;

public sealed class CpfValidator : ICpfValidator
{
    public bool IsValid(string cpf)
    {
        var digits = OnlyDigits(cpf);

        if (digits.Length != 11)
            return false;

        if (digits.Distinct().Count() == 1)
            return false;

        var firstDigit = CalculateDigit(digits[..9], 10);
        var secondDigit = CalculateDigit(digits[..10], 11);

        return digits[9] == firstDigit && digits[10] == secondDigit;
    }

    public string OnlyDigits(string cpf)
    {
        return new string((cpf ?? string.Empty).Where(char.IsDigit).ToArray());
    }

    private static char CalculateDigit(string baseDigits, int weight)
    {
        var sum = 0;

        foreach (var digit in baseDigits)
        {
            sum += (digit - '0') * weight;
            weight--;
        }

        var remainder = sum % 11;
        var result = remainder < 2 ? 0 : 11 - remainder;

        return (char)('0' + result);
    }
}
