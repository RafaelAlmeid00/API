
using Api.Domain;
using Api.Domain.Results;
using Api.Infrastructure;
using Api.Interface;
using Microsoft.EntityFrameworkCore;

namespace Api.Adapters_Repository
{

    public class UserRepository(EasyPassContext context) : IRepositoryUser
    {
        private readonly EasyPassContext _context = context;

        public async Task<IResultadoOperacao<List<User>>> Search(User data)
        {
            ILink link = new Link
            { Rel = "search_user", Href = "/User", Method = "GET" };

            try
            {
                IQueryable<User> queryable = _context.Users.AsQueryable();

                IQueryable<User> filteredQueryable = queryable
                    .Where(a => data.UserCpf == null
                    || a.UserCpf == data.UserCpf)
                    .Where(a => string.IsNullOrEmpty(data.UserNome)
                    || a.UserNome == data.UserNome)
                    .Where(a => string.IsNullOrEmpty(data.UserEmail)
                    || a.UserEmail == data.UserEmail)
                    .Where(a => string.IsNullOrEmpty(data.UserRg)
                    || a.UserRg == data.UserRg)
                    .Where(a => string.IsNullOrEmpty(data.UserEndCep)
                    || a.UserEndCep == data.UserEndCep)
                    .Where(a => string.IsNullOrEmpty(data.UserEndcidade)
                    || a.UserEndcidade == data.UserEndcidade)
                    .Where(a => string.IsNullOrEmpty(data.UserEndUf)
                    || a.UserEndUf == data.UserEndUf)
                    .Where(a => string.IsNullOrEmpty(data.UserEndbairro)
                    || a.UserEndbairro == data.UserEndbairro)
                    .Where(a => string.IsNullOrEmpty(data.UserEndrua)
                    || a.UserEndrua == data.UserEndrua)
                    .Where(a => string.IsNullOrEmpty(data.UserEndnum)
                    || a.UserEndnum == data.UserEndnum)
                    .Where(a => string.IsNullOrEmpty(data.UserEndcomplemento)
                    || a.UserEndcomplemento == data.UserEndcomplemento)
                    .Where(a => data.UserTipo == null
                    || a.UserTipo == data.UserTipo);


                List<User> Users = await filteredQueryable.ToListAsync();
                return Users.Count == 0 ? new ResultadoOperacao<List<User>>
                { Sucesso = false, Erro = "Nenhum User encontrado", Link = link }
                : new ResultadoOperacao<List<User>>
                { Data = Users, Sucesso = true, Link = link };

            }
            catch (DbUpdateException)
            {
                return new ResultadoOperacao<List<User>> { Sucesso = false, Erro = "Erro ao buscar Useres", Link = link };
            }
        }

        public async Task<IResultadoOperacao<User>> Create(User data)
        {
            ILink link = new Link
            { Rel = "create_user", Href = "/User", Method = "POST" };
            try
            {
                await _context.Users.AddAsync(data);
                await _context.SaveChangesAsync();
                IResultadoOperacao<List<User>> user = await Search(data);
                return user?.Data?.Count != 0
                ? new ResultadoOperacao<User>
                { Data = user?.Data?[0], Sucesso = true, Link = link }
                : new ResultadoOperacao<User>
                { Sucesso = false, Erro = "Erro ao salvar User", Link = link };
            }
            catch (DbUpdateException)
            {
                return await UserExist(data.UserCpf) ? new ResultadoOperacao<User>
                { Sucesso = false, Erro = "User já existe", Link = link }
                : new ResultadoOperacao<User>
                { Sucesso = false, Erro = "Erro ao salvar User", Link = link };
            }
        }
        
        // public byte[] GeneratePDFUsingReportBuilder(User userData)
        // {
        //     var report = new ReportBuilder();
        //     report.AddSection("Informações do Usuário");
        //     report.AddParagraph($"Nome: {userData.Name}");
        //     report.AddParagraph($"Email: {userData.Email}");
        //     return report.RenderPDF();
        // }

        public async Task<IResultadoOperacao<User>> GetOne(User data)
        {
            ILink link = new Link
            { Rel = "get_user", Href = $"/User/{data.UserCpf}", Method = "GET" };

            try
            {
                User? user = await _context.Users.FindAsync(data.UserCpf);
                return user is not null
                ? new ResultadoOperacao<User>
                { Data = user, Sucesso = true, Link = link }
                : new ResultadoOperacao<User>
                { Sucesso = false, Erro = "User não existe", Link = link };
            }
            catch (DbUpdateException)
            {
                return await UserExist(data.UserCpf) ? new ResultadoOperacao<User>
                { Sucesso = false, Erro = "Erro ao buscar User", Link = link }
                : new ResultadoOperacao<User>
                { Sucesso = false, Erro = "User não existe", Link = link };
            }
        }

        public async Task<IResultadoOperacao<User>> Edit(User data)
        {
            ILink link = new Link
            { Rel = "edit_user", Href = "/User", Method = "PUT" };
            try
            {
                User? adm = await _context.Users.FindAsync(data.UserCpf);
                if (adm is not null)
                {
                    var properties = typeof(User).GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.CanWrite)
                        {
                            var value = property.GetValue(data);
                            if (value != null)
                            {
                                property.SetValue(adm, value);
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
                IResultadoOperacao<User>? user = (IResultadoOperacao<User>?)await GetOne(data);
                return user is not null && user.Data is not null
                ? new ResultadoOperacao<User>
                { Data = user.Data, Sucesso = true, Link = link }
                : new ResultadoOperacao<User>
                { Sucesso = false, Erro = "Erro ao editar User", Link = link };
            }
            catch (DbUpdateException)
            {
                return await UserExist(data.UserCpf) ? new ResultadoOperacao<User>
                { Sucesso = false, Erro = "Erro ao editar User", Link = link }
                : new ResultadoOperacao<User>
                { Sucesso = false, Erro = "User não existe", Link = link };
            }
        }
        public async Task<IResultadoOperacao<User>> Delete(User data)
        {
            ILink link = new Link
            { Rel = "edit_user", Href = "/User", Method = "PUT" };
            try
            {
                User? buscaUser = await _context.Users.FindAsync(data.UserCpf);
                if (buscaUser == null)
                {
                    return new ResultadoOperacao<User>
                    { Sucesso = false, Erro = "User não existe", Link = link };
                }
                _context.Users.Remove(buscaUser);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<User>
                { Data = buscaUser, Sucesso = true, Link = link };
            }
            catch (DbUpdateException)
            {
                return await UserExist(data.UserCpf) ? new ResultadoOperacao<User>
                { Sucesso = false, Erro = "Erro ao deletar User", Link = link }
                : new ResultadoOperacao<User>
                { Sucesso = false, Erro = "User não existe!", Link = link };
            }
        }
        public async Task<IResultadoOperacao<User>> Disable(User data)
        {
            ILink link = new Link
            { Rel = "disable_user", Href = "/User/Disable", Method = "POST" };
            try
            {
                IResultadoOperacao<User> user = await GetOne(data);
                user.Data.UserStatus = "inativo";
                _context.Update(user);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<User>
                { Data = user.Data, Sucesso = true, Link = link };
            }
            catch (System.Exception)
            {
                return await UserExist(data.UserCpf) ? new ResultadoOperacao<User>
                { Sucesso = false, Erro = "Erro ao desabilitar User", Link = link }
                : new ResultadoOperacao<User>
                { Sucesso = false, Erro = "User não existe!", Link = link };
            }
        }

        public async Task<IResultadoOperacao<User>> Enable(User data)
        {
            ILink link = new Link
            { Rel = "disable_user", Href = "/User/Disable", Method = "POST" };
            try
            {
                IResultadoOperacao<User> user = await GetOne(data);
                user.Data.UserStatus = "ativo";
                _context.Update(user);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<User>
                { Data = user.Data, Sucesso = true, Link = link };
            }
            catch (System.Exception)
            {
                return await UserExist(data.UserCpf) ? new ResultadoOperacao<User>
                { Sucesso = false, Erro = "Erro ao desabilitar User", Link = link }
                : new ResultadoOperacao<User>
                { Sucesso = false, Erro = "User não existe!", Link = link };
            }
        }

        public async Task<IResultadoOperacao<User>> AlterType(User data)
        {
            ILink link = new Link
            { Rel = "disable_user", Href = "/User/AlterType", Method = "POST" };
            try
            {
                IResultadoOperacao<User> user = await GetOne(data);
                user.Data.UserTipo = data.UserTipo;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<User>
                { Data = user.Data, Sucesso = true, Link = link };
            }
            catch (System.Exception)
            {
                return await UserExist(data.UserCpf) ? new ResultadoOperacao<User>
                { Sucesso = false, Erro = "Erro ao desabilitar User", Link = link }
                : new ResultadoOperacao<User>
                { Sucesso = false, Erro = "User não existe!", Link = link };
            }
        }

        private async Task<bool> UserExist(string? cpf)
        {
            return await _context.Users.AnyAsync(e => e.UserCpf == cpf);
        }

    }
}