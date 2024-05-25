using Api.Domain;

namespace Api.Interface;
 public interface IRepositoryCard : IBaseRepository<Card>, IDeleteEditRepository<Card>, IAlterTypeRepository<Card>
    {
        Task<IResultadoOperacao<dynamic>> CreateWithExcel((List<dynamic>, List<dynamic>) data);
    }