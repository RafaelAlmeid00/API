using Api.Domain;
using Api.Interface;

namespace Api.Services
{
    public class ServiceBussines(IRepositoryBussines Repository, ICnpjValidator validator, ICrypto Crypto, IAuth Auth, IHttpContextAccessor httpContextAccessor) : IServiceBussines
    {
        private readonly IRepositoryBussines _Repository = Repository;
        private readonly ICrypto _Crypto = Crypto;
        private readonly IAuth _Auth = Auth;
        private readonly ICnpjValidator _Validator = validator;

        public async Task<IResultadoOperacao<Bussines>> AlterType(Bussines data)
        {
            return await _Repository.AlterType(data);
        }

        public async Task<IResultadoOperacao<Bussines>> Create(Bussines data)
        {
            ILink link = new Link { Rel = "create_admin", Href = "/Bussines/Login", Method = "POST" };

            if (!string.IsNullOrEmpty(data.BussSenha))
            {
                if (_Validator.ValidateCNPJ(data.BussCnpj))
                {
                    string hash = _Crypto.Encrypt(data.BussSenha);
                    data.BussSenha = hash;
                    return await _Repository.Create(data);
                }
                return new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "CNPJ inválido", Link = link };
            }
            return new ResultadoOperacao<Bussines>
            { Sucesso = false, Erro = "Senha inválida", Link = link };
        }

        public async Task<IResultadoOperacao<Bussines>> Delete(Bussines data)
        {
            return await _Repository.Delete(data);
        }

        public async Task<IResultadoOperacao<Bussines>> Disable(Bussines data)
        {
            return await _Repository.Disable(data);
        }

        public async Task<IResultadoOperacao<Bussines>> Edit(Bussines data)
        {
            ILink link = new Link
            { Rel = "edit_bussines", Href = "/Bussines", Method = "PUT" };

            if (!string.IsNullOrEmpty(data.BussCnpj) && !_Validator.ValidateCNPJ(data.BussCnpj))
            {
                return new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "CNPJ inválido", Link = link };
            }
            if (!string.IsNullOrEmpty(data.BussSenha))
            {
                string hash = _Crypto.Encrypt(data.BussSenha);
                data.BussSenha = hash;
            }
            return await _Repository.Edit(data);
        }

        public async Task<IResultadoOperacao<Bussines>> Enable(Bussines data)
        {
            return await _Repository.Enable(data);
        }

        public async Task<IResultadoOperacao<Bussines>> GetOne(Bussines data)
        {
            return await _Repository.GetOne(data);
        }

        public async Task<IResultadoOperacao<object>> Login(IBussinesLoginDTO data)
        {
            ILink link = new Link { Rel = "login_admin", Href = "/Bussines/Login", Method = "POST" };
            Bussines? admin = new()
            {
                BussCnpj = data.BussCnpj
            };
            IResultadoOperacao<List<Bussines>> search = await Search(admin);
            if (search.Data is not null && search.Data[0] is not null && !string.IsNullOrEmpty(data.BussSenha))
            {
                bool compare = _Crypto.Decrypt(data.BussSenha, search.Data[0].BussSenha ?? "");
                if (compare)
                {
                    string token = _Auth.CreateTokenBussines(search.Data[0]);
                    object result = new
                    {
                        Token = token,
                        Bussines = search.Data[0]
                    };
                    return new ResultadoOperacao<object>
                    { Data = result, Sucesso = true, Link = link };
                }
                return new ResultadoOperacao<object>
                { Sucesso = false, Erro = "Email ou Senha Incorreta", Link = link };
            }
            return new ResultadoOperacao<object>
            { Sucesso = false, Erro = "Email ou Senha Incorreta", Link = link };
        }

        public async Task<IResultadoOperacao<List<Bussines>>> Search(Bussines data)
        {
            return await _Repository.Search(data);
        }
    }
}