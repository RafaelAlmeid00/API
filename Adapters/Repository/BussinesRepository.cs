
using Api.Domain;
using Api.Infrastructure;
using Api.Interface;
using Microsoft.EntityFrameworkCore;

namespace Api.Adapters_Repository
{

    public class BussinesRepository(EasyPassContext context) : IRepositoryBussines
    {
        private readonly EasyPassContext _context = context;

        public async Task<IResultadoOperacao<List<Bussines>>> Search(Bussines data)
        {
            ILink link = new Link
            { Rel = "search_admin", Href = "/Bussines", Method = "GET" };

            try
            {
                IQueryable<Bussines> queryable = _context.Bussines.AsQueryable();

                IQueryable<Bussines> filteredQueryable = queryable
                    .Where(a => data.BussCnpj == null
                    || a.BussCnpj == data.BussCnpj)
                    .Where(a => string.IsNullOrEmpty(data.BussNome)
                    || a.BussNome == data.BussNome)
                    .Where(a => string.IsNullOrEmpty(data.BussEmail)
                    || a.BussEmail == data.BussEmail)
                    .Where(a => string.IsNullOrEmpty(data.BussEndCep)
                    || a.BussEndCep == data.BussEndCep)
                    .Where(a => string.IsNullOrEmpty(data.BussEndcidade)
                    || a.BussEndcidade == data.BussEndcidade)
                    .Where(a => string.IsNullOrEmpty(data.BussEndUf)
                    || a.BussEndUf == data.BussEndUf)
                    .Where(a => string.IsNullOrEmpty(data.BussEndbairro)
                    || a.BussEndbairro == data.BussEndbairro)
                    .Where(a => string.IsNullOrEmpty(data.BussEndrua)
                    || a.BussEndrua == data.BussEndrua)
                    .Where(a => string.IsNullOrEmpty(data.BussEndnum)
                    || a.BussEndnum == data.BussEndnum)
                    .Where(a => string.IsNullOrEmpty(data.BussEndcomplemento)
                    || a.BussEndcomplemento == data.BussEndcomplemento)
                    .Where(a => data.BussTipo == null
                    || a.BussTipo == data.BussTipo)
                    .Where(a => data.BussStatus == null
                    || a.BussStatus == data.BussStatus);


                List<Bussines> Bussines = await filteredQueryable.ToListAsync();
                return Bussines.Count == 0 ? new ResultadoOperacao<List<Bussines>>
                { Sucesso = false, Erro = "Nenhum Bussines encontrado", Link = link }
                : new ResultadoOperacao<List<Bussines>>
                { Data = Bussines, Sucesso = true, Link = link };

            }
            catch (DbUpdateException)
            {
                return new ResultadoOperacao<List<Bussines>> { Sucesso = false, Erro = "Erro ao buscar Admines", Link = link };
            }
        }

        public async Task<IResultadoOperacao<Bussines>> Create(Bussines data)
        {
            ILink link = new Link
            { Rel = "create_admin", Href = "/Bussines", Method = "POST" };
            try
            {
                await _context.Bussines.AddAsync(data);
                await _context.SaveChangesAsync();
                IResultadoOperacao<List<Bussines>> admin = await Search(data);
                return admin?.Data?.Count != 0
                ? new ResultadoOperacao<Bussines>
                { Data = admin?.Data?[0], Sucesso = true, Link = link }
                : new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Erro ao salvar Bussines", Link = link };
            }
            catch (DbUpdateException)
            {
                return await AdminExists(data.BussCnpj) ? new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Bussines já existe", Link = link }
                : new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Erro ao salvar Bussines", Link = link };
            }
        }


        public async Task<IResultadoOperacao<Bussines>> GetOne(Bussines data)
        {
            ILink link = new Link
            { Rel = "get_admin", Href = $"/Bussines/{data.BussCnpj}", Method = "GET" };

            try
            {
                Bussines? admin = await _context.Bussines.FindAsync(data.BussCnpj);
                return admin is not null
                ? new ResultadoOperacao<Bussines>
                { Data = admin, Sucesso = true, Link = link }
                : new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Bussines não existe", Link = link };
            }
            catch (DbUpdateException)
            {
                return await AdminExists(data.BussCnpj) ? new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Erro ao buscar Bussines", Link = link }
                : new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Bussines não existe", Link = link };
            }
        }

        public async Task<IResultadoOperacao<Bussines>> Edit(Bussines data)
        {
            ILink link = new Link
            { Rel = "edit_admin", Href = "/Bussines", Method = "PUT" };
            try
            {
                Bussines? adm = await _context.Bussines.FindAsync(data.BussCnpj);
                if (adm is not null)
                {
                    var properties = typeof(Bussines).GetProperties();
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
                IResultadoOperacao<Bussines>? admin = (IResultadoOperacao<Bussines>?)await GetOne(data);
                return admin is not null && admin.Data is not null
                ? new ResultadoOperacao<Bussines>
                { Data = admin.Data, Sucesso = true, Link = link }
                : new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Erro ao editar Bussines", Link = link };
            }
            catch (DbUpdateException)
            {
                return await AdminExists(data.BussCnpj) ? new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Erro ao editar Bussines", Link = link }
                : new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Bussines não existe", Link = link };
            }
        }
        public async Task<IResultadoOperacao<Bussines>> Delete(Bussines data)
        {
            ILink link = new Link
            { Rel = "edit_admin", Href = "/Bussines", Method = "PUT" };
            try
            {
                Bussines? buscaAdmin = await _context.Bussines.FindAsync(data.BussCnpj);
                if (buscaAdmin == null)
                {
                    return new ResultadoOperacao<Bussines>
                    { Sucesso = false, Erro = "Bussines não existe", Link = link };
                }
                _context.Bussines.Remove(buscaAdmin);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<Bussines>
                { Data = buscaAdmin, Sucesso = true, Link = link };
            }
            catch (DbUpdateException)
            {
                return await AdminExists(data.BussCnpj) ? new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Erro ao deletar Bussines", Link = link }
                : new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Bussines não existe!", Link = link };
            }
        }
        public async Task<IResultadoOperacao<Bussines>> Disable(Bussines data)
        {
            ILink link = new Link
            { Rel = "disable_admin", Href = "/Bussines/Disable", Method = "POST" };
            try
            {
                IResultadoOperacao<Bussines> Bussines = await GetOne(data);
                Bussines.Data.BussStatus = "inativo";
                _context.Update(Bussines);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<Bussines>
                { Data = Bussines.Data, Sucesso = true, Link = link };
            }
            catch (System.Exception)
            {
                return await AdminExists(data.BussCnpj) ? new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Erro ao desabilitar Bussines", Link = link }
                : new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Bussines não existe!", Link = link };
            }
        }

        public async Task<IResultadoOperacao<Bussines>> Enable(Bussines data)
        {
            ILink link = new Link
            { Rel = "disable_admin", Href = "/Bussines/Disable", Method = "POST" };
            try
            {
                IResultadoOperacao<Bussines> Bussines = await GetOne(data);
                Bussines.Data.BussStatus = "ativo";
                _context.Update(Bussines);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<Bussines>
                { Data = Bussines.Data, Sucesso = true, Link = link };
            }
            catch (System.Exception)
            {
                return await AdminExists(data.BussCnpj) ? new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Erro ao desabilitar Bussines", Link = link }
                : new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Bussines não existe!", Link = link };
            }
        }

        public async Task<IResultadoOperacao<Bussines>> AlterType(Bussines data)
        {
            ILink link = new Link
            { Rel = "disable_admin", Href = "/Bussines/Disable", Method = "POST" };
            try
            {
                IResultadoOperacao<Bussines> Bussines = await GetOne(data);
                Bussines.Data.BussTipo = data.BussTipo;
                _context.Update(Bussines);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<Bussines>
                { Data = Bussines.Data, Sucesso = true, Link = link };
            }
            catch (System.Exception)
            {
                return await AdminExists(data.BussCnpj) ? new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Erro ao desabilitar Bussines", Link = link }
                : new ResultadoOperacao<Bussines>
                { Sucesso = false, Erro = "Bussines não existe!", Link = link };
            }
        }

        private async Task<bool> AdminExists(string? cpf)
        {
            return await _context.Bussines.AnyAsync(e => e.BussCnpj == cpf);
        }

    }
}