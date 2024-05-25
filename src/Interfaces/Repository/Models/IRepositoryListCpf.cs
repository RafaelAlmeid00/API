using Api.Domain;

namespace Api.Interface;
 public interface IRepositoryListCpf : IBaseRepository<ListCpf>, IDeleteEditRepository<ListCpf>, IAlterTypeRepository<ListCpf>
    {
        Task<IResultadoOperacao<dynamic>> CreateWithExcel((List<dynamic>, List<dynamic>) data);
    }
    