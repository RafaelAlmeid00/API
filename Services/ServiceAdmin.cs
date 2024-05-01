using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authentication;

namespace Api.Services
{
    public class ServiceAdmin(IRepositoryAdmin<Admin> Repository, ICrypto Crypto, IAuth Auth, IHttpContextAccessor httpContextAccessor) : IServiceAdmin<Admin>
    {
        private readonly IRepositoryAdmin<Admin> _Repository = Repository;
        private readonly ICrypto _Crypto = Crypto;
        private readonly IAuth _Auth = Auth;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<IResultadoOperacao<Admin>> Create(Admin data)
        {
            ILink link = new Link { Rel = "create_admin", Href = "/Admin/Login", Method = "POST" };

            if (!string.IsNullOrEmpty(data.AdmSenha))
            {
                string hash = _Crypto.Encrypt(data.AdmSenha);
                data.AdmSenha = hash;
                return await _Repository.Create(data);
            }
            return new ResultadoOperacao<Admin>
            { Sucesso = false, Erro = "Senha inválida", Link = link };
        }

        public async Task<IResultadoOperacao<Admin>> Delete(Admin data)
        {
            return await _Repository.Delete(data);
        }

        public async Task<IResultadoOperacao<Admin>> Edit(Admin data)
        {
            if (!string.IsNullOrEmpty(data.AdmSenha))
            {
                string hash = _Crypto.Encrypt(data.AdmSenha);
                data.AdmSenha = hash;
            }
            return await _Repository.Edit(data);
        }

        public async Task<IResultadoOperacao<Admin>> GetOne(Admin data)
        {
            return await _Repository.GetOne(data);
        }

        public async Task<IResultadoOperacao<object>> Login(IAdminLoginDTO data)
        {
            ILink link = new Link { Rel = "login_admin", Href = "/Admin/Login", Method = "POST" };
            Admin? admin = new()
            {
                AdmEmail = data.AdmEmail
            };
            IResultadoOperacao<List<Admin>> search = await Search(admin);
            if (search.Data is not null && search.Data[0] is not null && !string.IsNullOrEmpty(data.AdmSenha))
            {
                bool compare = _Crypto.Decrypt(data.AdmSenha, search.Data[0].AdmSenha ?? "");
                if (compare)
                {
                    string token = _Auth.CreateTokenAdmin(search.Data[0]);
                    object result = new {
                        Token = token,
                        Admin = search.Data[0]
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

        public IResultadoOperacao<string> Logout(IAdminLoginDTO data)
        {
            ILink link = new Link { Rel = "logout_admin", Href = "/Admin/Logout", Method = "POST" };

            string? token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");

            if (token is not null)
            {
                _httpContextAccessor?.HttpContext?.Session.Clear();
                return new ResultadoOperacao<string> { Sucesso = true, Link = link };
            }
            return new ResultadoOperacao<string> { Sucesso = false, Erro = "Token não encontrado", Link = link };
        }

        public async Task<IResultadoOperacao<List<Admin>>> Search(Admin data)
        {
            return await _Repository.Search(data);
        }
    }
}