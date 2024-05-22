
using Api.Interface;
using Newtonsoft.Json;

namespace Api.Services
{
    public class ListCPFValidator(ICpfValidator cpfValidator, ICnpjValidator cnpjValidator) : IListCPFValidator
    {
        private static dynamic TransformItem(dynamic item)
        {
            return new
            {
                BussinesBussCnpj = item.cnpj,
                ListTipo = item.tipo,
                ListCPF = item.cpf
            };
        }

        private bool IsValidCPF(string cpf)
        {
            return cpfValidator.ValidateCPF(cpf);
        }

        private bool IsValidCNPJ(string cnpj)
        {
            return cnpjValidator.ValidateCNPJ(cnpj);
        }

        private static bool IsValidType(string tipo)
        {
            return tipo == "student" || tipo == "worker";
        }

        public async Task<(List<dynamic> arrayValid, List<dynamic> arrayInvalid)> ValidateAll(IFormFile data)
        {
            List<dynamic> arrayValid = [];
            List<dynamic> arrayInvalid = [];


            using (var reader = new StreamReader(data.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    string line = await reader.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        dynamic item = JsonConvert.DeserializeObject<dynamic>(line);
                        dynamic transformedItem = TransformItem(item);

                        bool isValidCPF = IsValidCPF(transformedItem.list_CPF);
                        bool isValidCNPJ = IsValidCNPJ(transformedItem.bussines_buss_CNPJ);
                        bool isValidType = IsValidType(transformedItem.list_tipo);

                        Task.WhenAll(transformedItem, isValidCPF, isValidCNPJ, isValidType);

                        if (isValidCPF && isValidCNPJ && isValidType)
                        {
                            arrayValid.Add(transformedItem);
                        }
                        else
                        {
                            arrayInvalid.Add(transformedItem);
                        }
                    }
                }
            }
            return (arrayValid, arrayInvalid);
        }
    }

}