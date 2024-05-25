using Api.Domain;
using Api.Interface;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services
{
    public class ServiceCard : IServiceCard
    {
        private readonly IRepositoryCard _repository;

        public ServiceCard(IRepositoryCard repository)
        {
            _repository = repository;
        }

        public async Task<IResultadoOperacao<Card>> Create(Card data)
        {
            return await _repository.Create(data);
        }

        public async Task<IResultadoOperacao<dynamic>> CreateWithExcel(IFormFile data)
        {
            (List<dynamic>, List<dynamic>) excelData = await ParseExcelFile(data);
            return await _repository.CreateWithExcel(excelData);
        }

        public async Task<IResultadoOperacao<Card>> Delete(Card data)
        {
            return await _repository.Delete(data);
        }

        public async Task<IResultadoOperacao<Card>> Edit(Card data)
        {
            return await _repository.Edit(data);
        }

        public async Task<IResultadoOperacao<Card>> GetOne(Card data)
        {
            return await _repository.GetOne(data);
        }

        public async Task<IResultadoOperacao<List<Card>>> Search(Card data)
        {
            return await _repository.Search(data);
        }

        public async Task<IResultadoOperacao<Card>> AlterType(Card data)
        {
            return await _repository.AlterType(data);
        }

        private async Task<(List<dynamic>, List<dynamic>)> ParseExcelFile(IFormFile data)
        {
            List<dynamic> valids = new List<dynamic>();
            List<dynamic> invalids = new List<dynamic>();
            

            return (valids, invalids);
        }
    }
}
