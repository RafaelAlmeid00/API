
using Api.Domain;
using Api.Domain.Results;
using Api.Infrastructure;
using Api.Interface;
using Microsoft.EntityFrameworkCore;

namespace Api.Adapters_Repository
{

    public class ListCpfRepository(EasyPassContext context) : IRepositoryListCpf
    {
        private readonly EasyPassContext _context = context;

        public async Task<IResultadoOperacao<List<ListCpf>>> Search(ListCpf data)
        {
            ILink link = new Link
            { Rel = "search_ListCpf", Href = "/ListCpf", Method = "GET" };

            try
            {
                IQueryable<ListCpf> queryable = _context.ListCpfs.AsQueryable();

                IQueryable<ListCpf> filteredQueryable = queryable
                    .Where(a => data.ListId == null
                    || a.ListId == data.ListId)
                    .Where(a => string.IsNullOrEmpty(data.ListTipo)
                    || a.ListTipo == data.ListTipo)
                    .Where(a => string.IsNullOrEmpty(data.ListCPF)
                    || a.ListCPF == data.ListCPF)
                    .Where(a => data.BussinesBussCnpj == null
                    || a.BussinesBussCnpj == data.BussinesBussCnpj);

                List<ListCpf> ListCpfs = await filteredQueryable.ToListAsync();
                return ListCpfs.Count == 0 ? new ResultadoOperacao<List<ListCpf>>
                { Sucesso = false, Erro = "Nenhum ListCpf encontrado", Link = link }
                : new ResultadoOperacao<List<ListCpf>>
                { Data = ListCpfs, Sucesso = true, Link = link };

            }
            catch (DbUpdateException)
            {
                return new ResultadoOperacao<List<ListCpf>> { Sucesso = false, Erro = "Erro ao buscar ListCpfes", Link = link };
            }
        }

        public async Task<IResultadoOperacao<ListCpf>> Create(ListCpf data)
        {
            ILink link = new Link
            { Rel = "create_ListCpf", Href = "/ListCpf", Method = "POST" };
            try
            {
                await _context.ListCpfs.AddAsync(data);
                await _context.SaveChangesAsync();
                IResultadoOperacao<List<ListCpf>> ListCpf = await Search(data);
                return ListCpf?.Data?.Count != 0
                ? new ResultadoOperacao<ListCpf>
                { Data = ListCpf?.Data?[0], Sucesso = true, Link = link }
                : new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "Erro ao salvar ListCpf", Link = link };
            }
            catch (DbUpdateException)
            {
                return await ListCpfExists(data.ListCPF) ? new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "ListCpf já existe", Link = link }
                : new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "Erro ao salvar ListCpf", Link = link };
            }
        }

        public async Task<IResultadoOperacao<dynamic>> CreateWithExcel((List<dynamic>, List<dynamic>) data)
        {
            ILink link = new Link
            { Rel = "create_ListCpf", Href = "/ListCpf", Method = "POST" };

            List<dynamic> itensSaved = [];
            List<dynamic> result =
            [
                new { saved = itensSaved, valids = data.Item1, invalids = data.Item2 }
            ];

            try
            {
                foreach (var item in data.Item1)
                {
                    await _context.ListCpfs.AddAsync(item);
                    await _context.SaveChangesAsync();
                    IResultadoOperacao<List<ListCpf>> ListCpf = await Search(item);
                    itensSaved.Add(ListCpf.Data[0]);
                }

                return itensSaved.Count != 0
                ? new ResultadoOperacao<dynamic>
                { Data = result, Sucesso = true, Link = link }
                : new ResultadoOperacao<dynamic>
                { Data = result, Sucesso = false, Erro = "Erro ao salvar ListCpf", Link = link };
            }
            catch (DbUpdateException)
            {
                return new ResultadoOperacao<dynamic>
                { Data = result, Sucesso = false, Erro = "Erro ao salvar ListCpf", Link = link };
            }
        }

        public async Task<IResultadoOperacao<ListCpf>> GetOne(ListCpf data)
        {
            ILink link = new Link
            { Rel = "get_ListCpf", Href = $"/ListCpf/{data.ListCPF}", Method = "GET" };

            try
            {
                ListCpf? ListCpf = await _context.ListCpfs.FindAsync(data.ListCPF);
                return ListCpf is not null
                ? new ResultadoOperacao<ListCpf>
                { Data = ListCpf, Sucesso = true, Link = link }
                : new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "ListCpf não existe", Link = link };
            }
            catch (DbUpdateException)
            {
                return await ListCpfExists(data.ListCPF) ? new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "Erro ao buscar ListCpf", Link = link }
                : new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "ListCpf não existe", Link = link };
            }
        }

        public async Task<IResultadoOperacao<ListCpf>> Edit(ListCpf data)
        {
            ILink link = new Link
            { Rel = "edit_ListCpf", Href = "/ListCpf", Method = "PUT" };
            try
            {
                ListCpf? adm = await _context.ListCpfs.FindAsync(data.ListCPF);
                if (adm is not null)
                {
                    var properties = typeof(ListCpf).GetProperties();
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
                IResultadoOperacao<ListCpf>? ListCpf = (IResultadoOperacao<ListCpf>?)await GetOne(data);
                return ListCpf is not null && ListCpf.Data is not null
                ? new ResultadoOperacao<ListCpf>
                { Data = ListCpf.Data, Sucesso = true, Link = link }
                : new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "Erro ao editar ListCpf", Link = link };
            }
            catch (DbUpdateException)
            {
                return await ListCpfExists(data.ListCPF) ? new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "Erro ao editar ListCpf", Link = link }
                : new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "ListCpf não existe", Link = link };
            }
        }
        public async Task<IResultadoOperacao<ListCpf>> Delete(ListCpf data)
        {
            ILink link = new Link
            { Rel = "edit_ListCpf", Href = "/ListCpf", Method = "PUT" };
            try
            {
                ListCpf? buscaListCpf = await _context.ListCpfs.FindAsync(data.ListCPF);
                if (buscaListCpf == null)
                {
                    return new ResultadoOperacao<ListCpf>
                    { Sucesso = false, Erro = "ListCpf não existe", Link = link };
                }
                _context.ListCpfs.Remove(buscaListCpf);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<ListCpf>
                { Data = buscaListCpf, Sucesso = true, Link = link };
            }
            catch (DbUpdateException)
            {
                return await ListCpfExists(data.ListCPF) ? new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "Erro ao deletar ListCpf", Link = link }
                : new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "ListCpf não existe!", Link = link };
            }
        }

        public async Task<IResultadoOperacao<ListCpf>> AlterType(ListCpf data)
        {
            ILink link = new Link
            { Rel = "disable_ListCpf", Href = "/ListCpf/AlterType", Method = "POST" };
            try
            {
                IResultadoOperacao<ListCpf> ListCpf = await GetOne(data);
                ListCpf.Data.ListTipo = data.ListTipo;
                _context.Update(ListCpf);
                await _context.SaveChangesAsync();
                return new ResultadoOperacao<ListCpf>
                { Data = ListCpf.Data, Sucesso = true, Link = link };
            }
            catch (System.Exception)
            {
                return await ListCpfExists(data.ListCPF) ? new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "Erro ao desabilitar ListCpf", Link = link }
                : new ResultadoOperacao<ListCpf>
                { Sucesso = false, Erro = "ListCpf não existe!", Link = link };
            }
        }

        private async Task<bool> ListCpfExists(string? cpf)
        {
            return await _context.ListCpfs.AnyAsync(e => e.ListCPF == cpf);
        }
    }
}