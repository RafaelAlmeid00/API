using Api.Domain;
using Api.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services
{
    public class ServiceRequestCard : IServiceRequestCard
    {
        private readonly IRepositoryRequestCard _repository;

        public ServiceRequestCard(IRepositoryRequestCard repository)
        {
            _repository = repository;
        }

        public async Task<IResultadoOperacao<List<RequestCard>>> Search(RequestCard data)
        {
            return await _repository.Search(data);
        }

        public async Task<IResultadoOperacao<RequestCard>> GetOne(RequestCard data)
        {
            return await _repository.GetOne(data);
        }

        public async Task<IResultadoOperacao<RequestCard>> Create(RequestCard data)
        {
            return await _repository.Create(data);
        }

        public async Task<IResultadoOperacao<RequestCard>> Delete(RequestCard data)
        {
            return await _repository.Delete(data);
        }

        public async Task<IResultadoOperacao<RequestCard>> Edit(RequestCard data)
        {
            return await _repository.Edit(data);
        }

        public async Task<IResultadoOperacao<RequestCard>> AlterType(RequestCard data)
        {
            return await _repository.AlterType(data);
        }
    }
}