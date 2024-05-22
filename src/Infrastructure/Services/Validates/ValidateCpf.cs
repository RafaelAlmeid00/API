using System.Text.RegularExpressions;
using Api.Interface;

namespace Api.Services
{
    public partial class CpfValidator : ICpfValidator
    {
        private static readonly int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        private static readonly int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        public bool ValidateCPF(string cpf)
        {
            cpf = CleanCpf(cpf);
            if (!IsValidLength(cpf) || IsBlockedSequence(cpf))
            {
                return false;
            }

            string tempCpf = cpf[..9];
            string calculatedCpf = CalculateCpfDigits(tempCpf);

            return cpf.EndsWith(calculatedCpf);
        }

        private static string CleanCpf(string cpf)
        {
            return MyRegex().Replace(cpf, "");
        }

        private static bool IsValidLength(string cpf)
        {
            return cpf.Length == 11;
        }

        private static bool IsBlockedSequence(string cpf)
        {
            string[] blockedSequences =
            [
            "00000000000", "11111111111", "22222222222", "33333333333",
            "44444444444", "55555555555", "66666666666", "77777777777",
            "88888888888", "99999999999"
            ];

            return blockedSequences.Contains(cpf);
        }

        private static string CalculateCpfDigits(string cpf)
        {
            int firstDigit = CalculateDigit(cpf, multiplicador1);
            int secondDigit = CalculateDigit(cpf + firstDigit, multiplicador2);

            return firstDigit.ToString() + secondDigit.ToString();
        }

        private static int CalculateDigit(string cpf, int[] multiplicador)
        {
            int soma = 0;
            for (int i = 0; i < multiplicador.Length; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * multiplicador[i];
            }

            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }

        [GeneratedRegex("[^0-9]")]
        private static partial Regex MyRegex();
    }

}