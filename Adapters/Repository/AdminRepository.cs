
using Api.Domain;
using Api.Infrastructure;
using Api.Interface;
using Microsoft.EntityFrameworkCore;

namespace Api.Adapters_Repository
{

    public class AdminRepository(EasyPassContext context) : IRepositoryAdmin
    {
        private readonly EasyPassContext _context = context;

        public async Task<IResultadoOperacao<List<Admin>>> Search(Admin data)
        {
            ILink link = new Link
            { Rel = "search_admin", Href = "/Admin", Method = "GET" };

            try
            {
                IQueryable<Admin> queryable = _context.Admins.AsQueryable();

                IQueryable<Admin> filteredQueryable = queryable
                    .Where(a => data.AdmId == null
                    || a.AdmId == data.AdmId)
                    .Where(a => string.IsNullOrEmpty(data.AdmNome)
                    || a.AdmNome == data.AdmNome)
                    .Where(a => string.IsNullOrEmpty(data.AdmEmail)
                    || a.AdmEmail == data.AdmEmail)
                    .Where(a => data.AdmLevel == null
                    || a.AdmLevel == data.AdmLevel);

                List<Admin> Admins = await filteredQueryable.ToListAsync();
                return Admins.Count == 0 ? new ResultadoOperacao<List<Admin>>
                { Sucesso = false, Erro = "Nenhum Admin encontrado", Link = link }
                : new ResultadoOperacao<List<Admin>>
                { Data = Admins, Sucesso = true, Link = link };

            }
            catch (DbUpdateException)
            {
                return new ResultadoOperacao<List<Admin>> { Sucesso = false, Erro = "Erro ao buscar Admines", Link = link };
            }
        }

        public async Task<IResultadoOperacao<Admin>> Create(Admin data)
        {
            ILink link = new Link
            { Rel = "create_admin", Href = "/Admin", Method = "POST" };
            try
            {
                await _context.Admins.AddAsync(data);
                await _context.SaveChangesAsync();
                IResultadoOperacao<List<Admin>> admin = await Search(data);
                return admin?.Data?.Count != 0
                ? new ResultadoOperacao<Admin>
                { Data = admin?.Data?[0], Sucesso = true, Link = link }
                : new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Erro ao salvar Admin", Link = link };
            }
            catch (DbUpdateException)
            {
                return await AdminExistsEmail(data.AdmEmail) ? new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Admin já existe", Link = link }
                : new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Erro ao salvar Admin", Link = link };
            }
        }


        public async Task<IResultadoOperacao<Admin>> GetOne(Admin data)
        {
            ILink link = new Link
            { Rel = "get_admin", Href = $"/Admin/{data.AdmId}", Method = "GET" };

            try
            {
                Admin? admin = await _context.Admins.FindAsync(data.AdmId);
                return admin is not null
                ? new ResultadoOperacao<Admin>
                { Data = admin, Sucesso = true, Link = link }
                : new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Admin não existe", Link = link };
            }
            catch (DbUpdateException)
            {
                return await AdminExists(data.AdmId) ? new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Erro ao buscar Admin", Link = link }
                : new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Admin não existe", Link = link };
            }
        }

        public async Task<IResultadoOperacao<Admin>> Edit(Admin data)
        {
            ILink link = new Link
            { Rel = "edit_admin", Href = "/Admin", Method = "PUT" };
            try
            {
                Admin? adm = await _context.Admins.FindAsync(data.AdmId);
                if (adm is not null)
                {
                    var properties = typeof(Admin).GetProperties();
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
                IResultadoOperacao<Admin>? admin = (IResultadoOperacao<Admin>?)await GetOne(data);
                return admin is not null && admin.Data is not null
                ? new ResultadoOperacao<Admin>
                { Data = admin.Data, Sucesso = true, Link = link }
                : new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Erro ao editar Admin", Link = link };
            }
            catch (DbUpdateException)
            {
                return await AdminExists(data.AdmId) ? new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Erro ao editar Admin", Link = link }
                : new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Admin não existe", Link = link };
            }
        }
        public async Task<IResultadoOperacao<Admin>> Delete(Admin data)
        {
            ILink link = new Link
            { Rel = "edit_admin", Href = "/Admin", Method = "PUT" };
            try
            {
                Admin? buscaAdmin = await _context.Admins.FindAsync(data.AdmId);
                if (buscaAdmin == null)
                {
                    return new ResultadoOperacao<Admin> 
                    { Sucesso = false, Erro = "Admin não existe", Link = link };
                }
                _context.Admins.Remove(buscaAdmin);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<Admin> 
                { Data = buscaAdmin, Sucesso = true, Link = link };
            }
            catch (DbUpdateException)
            {
                return await AdminExists(data.AdmId) ? new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Erro ao deletar Admin", Link = link }
                : new ResultadoOperacao<Admin>
                { Sucesso = false, Erro = "Admin não existe!", Link = link };
            }
        }
        private async Task<bool> AdminExists(int? id)
        {
            return await _context.Admins.AnyAsync(e => e.AdmId == id);
        }
        private async Task<bool> AdminExistsEmail(string? email)
        {
            return await _context.Admins.AnyAsync(e => e.AdmEmail == email);
        }
    }
}