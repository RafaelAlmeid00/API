using System.Text.RegularExpressions;
using Api.Interface;

namespace Api.Services
{
    public class CnpjValidator : ICnpjValidator
    {
        public bool ValidateCNPJ(string cnpj)
        {
            cnpj = CleanCNPJ(cnpj);

            if (!IsValidLength(cnpj) || IsRepeatedSequence(cnpj))
                return false;

            string tempCnpj = cnpj[..12];
            string firstDigit = CalculateDigit(tempCnpj, [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]);
            tempCnpj += firstDigit;

            string secondDigit = CalculateDigit(tempCnpj, [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]);

            return cnpj.EndsWith(firstDigit + secondDigit);
        }

        private static string CleanCNPJ(string cnpj)
        {
            return cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
        }

        private static bool IsValidLength(string cnpj)
        {
            return cnpj.Length == 14;
        }

        private static bool IsRepeatedSequence(string cnpj)
        {
            string[] invalidSequences =
            [
            "00000000000000", "11111111111111", "22222222222222",
            "33333333333333", "44444444444444", "55555555555555",
            "66666666666666", "77777777777777", "88888888888888",
            "99999999999999"
            ];

            return invalidSequences.Contains(cnpj);
        }

        private static string CalculateDigit(string cnpj, int[] multiplicador)
        {
            int soma = 0;

            for (int i = 0; i < multiplicador.Length; i++)
            {
                soma += int.Parse(cnpj[i].ToString()) * multiplicador[i];
            }

            int resto = soma % 11;
            if (resto < 2)
                return "0";
            else
                return (11 - resto).ToString();
        }
    }

}