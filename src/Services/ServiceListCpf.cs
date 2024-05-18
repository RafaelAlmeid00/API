using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Authentication;

namespace Api.Services
{
    public class ServiceListCpf(IRepositoryListCpf Repository, IListCPFValidator validator) : IServiceListCpf
    {
        private readonly IRepositoryListCpf _Repository = Repository;
        private readonly IListCPFValidator _Validator = validator;

        public async Task<IResultadoOperacao<ListCpf>> Create(ListCpf data)
        {
            return await _Repository.Create(data);
        }
        public async Task<IResultadoOperacao<dynamic>> CreateWithExcel(IFormFile data)
        {
            (List<dynamic>, List<dynamic>) excel = await _Validator.ValidateAll(data);
            return await _Repository.CreateWithExcel(excel);
        }
        public async Task<IResultadoOperacao<ListCpf>> Delete(ListCpf data)
        {
            return await _Repository.Delete(data);
        }

        public async Task<IResultadoOperacao<ListCpf>> Edit(ListCpf data)
        {
            return await _Repository.Edit(data);
        }

        public async Task<IResultadoOperacao<ListCpf>> GetOne(ListCpf data)
        {
            return await _Repository.GetOne(data);
        }
        public async Task<IResultadoOperacao<List<ListCpf>>> Search(ListCpf data)
        {
            return await _Repository.Search(data);
        }
        public async Task<IResultadoOperacao<ListCpf>> AlterType(ListCpf data)
        {
            return await _Repository.AlterType(data);
        }

    }
}