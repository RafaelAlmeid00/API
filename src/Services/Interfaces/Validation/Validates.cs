namespace Api.Interface
{
    public interface IListCPFValidator
    {
        Task<(List<dynamic> arrayValid, List<dynamic> arrayInvalid)> ValidateAll(IFormFile data);
}

    public interface ICpfValidator
    {
        bool ValidateCPF(string cpf);
    }

    public interface ICnpjValidator
    {
        bool ValidateCNPJ(string cnpj);
    }
}
