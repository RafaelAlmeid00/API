using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Api.Service
{
    public class Service(IRepository<IAdminDTO> Repository, ICrypto Crypto, IAuth<IAdminDTO> Auth, IHttpContextAccessor httpContextAccessor) : IBaseService<IAdminDTO>, IDeleteEditService<IAdminDTO>, ILoginLogoutService<IAdminLoginDTO>
    {
        private readonly IRepository<IAdminDTO> _Repository = Repository;
        private readonly ICrypto _Crypto = Crypto;
        private readonly IAuth<IAdminDTO> _Auth = Auth;
            private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<IResultadoOperacao<IAdminDTO>> Create(IAdminDTO data)
        {
            return await _Repository.Create(data);
        }

        public async Task<IResultadoOperacao<IAdminDTO>> Delete(IAdminDTO data)
        {
            return await _Repository.Delete(data);
        }

        public async Task<IResultadoOperacao<IAdminDTO>> Edit(IAdminDTO data)
        {
            return await _Repository.Edit(data);
        }

        public async Task<IResultadoOperacao<IAdminDTO>> GetOne(IAdminDTO data)
        {
            return await _Repository.GetOne(data);
        }

        public async Task<IResultadoOperacao<string>> Login(IAdminLoginDTO data)
        {
            ILink link = new Link { Rel = "login_admin", Href = "/Admin/Login", Method = "POST" };
            IResultadoOperacao<List<IAdminDTO>> search = await Search((IAdminDTO)data);
            if (search.Data is not null && search.Data[0] is not null && !string.IsNullOrEmpty(data.AdmSenha))
            {
                bool compare = _Crypto.Decrypt(data.AdmSenha, search.Data[0].AdmSenha);
                if (compare)
                {
                    string token = _Auth.CreateToken((IAdminDTO)search.Data);
                    return new ResultadoOperacao<string> { Data = token, Sucesso = true, Link = link };
                }
                return new ResultadoOperacao<string> { Sucesso = false, Erro = "CNPJ ou Senha Incorreta", Link = link };
            }
            return new ResultadoOperacao<string> { Sucesso = false, Erro = "CNPJ ou Senha Incorreta", Link = link };
        }

        public async Task<IResultadoOperacao<string>> Logout(IAdminLoginDTO data)
        {
            ILink link = new Link { Rel = "logout_admin", Href = "/Admin/Logout", Method = "POST" };

            string? token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");

            if (token is not null)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
                _httpContextAccessor.HttpContext.Session.Clear();
                return new ResultadoOperacao<string> { Sucesso = true, Link = link };
            }
            return new ResultadoOperacao<string> { Sucesso = false, Erro = "Token não encontrado", Link = link };
        }

        public async Task<IResultadoOperacao<List<IAdminDTO>>> Search(IAdminDTO data)
        {
            return await _Repository.Search(data);
        }
    }
}