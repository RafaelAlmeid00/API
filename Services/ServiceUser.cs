using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authentication;

namespace Api.Services
{
    public class ServiceUser(IRepositoryUser<User> Repository, ICrypto Crypto, IAuth Auth, IHttpContextAccessor httpContextAccessor) : IServiceUser<User>
    {
        private readonly IRepositoryUser<User> _Repository = Repository;
        private readonly ICrypto _Crypto = Crypto;
        private readonly IAuth _Auth = Auth;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<IResultadoOperacao<User>> AlterType(User data)
        {
            return await _Repository.AlterType(data);
        }

        public async Task<IResultadoOperacao<User>> Create(User data)
        {
            ILink link = new Link { Rel = "create_admin", Href = "/User/Login", Method = "POST" };

            if (!string.IsNullOrEmpty(data.UserSenha))
            {
                string hash = _Crypto.Encrypt(data.UserSenha);
                data.UserSenha = hash;
                return await _Repository.Create(data);
            }
            return new ResultadoOperacao<User>
            { Sucesso = false, Erro = "Senha inválida", Link = link };
        }

        public async Task<IResultadoOperacao<User>> Delete(User data)
        {
            return await _Repository.Delete(data);
        }

        public async Task<IResultadoOperacao<User>> Disable(User data)
        {
            return await _Repository.Disable(data);
        }

        public async Task<IResultadoOperacao<User>> Edit(User data)
        {
            if (!string.IsNullOrEmpty(data.UserSenha))
            {
                string hash = _Crypto.Encrypt(data.UserSenha);
                data.UserSenha = hash;
            }
            return await _Repository.Edit(data);
        }

        public async Task<IResultadoOperacao<User>> Enable(User data)
        {
            return await _Repository.Enable(data);
        }

        public async Task<IResultadoOperacao<User>> GetOne(User data)
        {
            return await _Repository.GetOne(data);
        }

        public async Task<IResultadoOperacao<object>> Login(IUserLoginDTO data)
        {
            ILink link = new Link { Rel = "login_admin", Href = "/User/Login", Method = "POST" };
            User? admin = new()
            {
                UserCpf = data.UserCpf
            };
            IResultadoOperacao<List<User>> search = await Search(admin);
            if (search.Data is not null && search.Data[0] is not null && !string.IsNullOrEmpty(data.UserSenha))
            {
                bool compare = _Crypto.Decrypt(data.UserSenha, search.Data[0].UserSenha ?? "");
                if (compare)
                {
                    string token = _Auth.CreateTokenUser(search.Data[0]);
                    object result = new
                    {
                        Token = token,
                        User = search.Data[0]
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

        public async Task<IResultadoOperacao<List<User>>> Search(User data)
        {
            return await _Repository.Search(data);
        }
    }
}