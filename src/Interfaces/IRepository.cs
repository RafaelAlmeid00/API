using Api.Domain;

namespace Api.Interface
{
    public interface IBaseRepository<T>
    {
        Task<IResultadoOperacao<List<T>>> Search(T data);
        Task<IResultadoOperacao<T>> GetOne(T data);
        Task<IResultadoOperacao<T>> Create(T data);
    }
    public interface IAlterTypeRepository<T>
    {
        Task<IResultadoOperacao<T>> AlterType(T data);
    }

    public interface IDeleteEditRepository<T>
    {
        Task<IResultadoOperacao<T>> Delete(T data);
        Task<IResultadoOperacao<T>> Edit(T data);
    }
    public interface IDisableEnableRepository<T>
    {
        Task<IResultadoOperacao<T>> Disable(T data);
        Task<IResultadoOperacao<T>> Enable(T data);
    }


    public interface IRepositoryAdmin : IBaseRepository<Admin>, IDeleteEditRepository<Admin>
    {
    }
    public interface IRepositoryUser : IBaseRepository<User>, IDeleteEditRepository<User>, IDisableEnableRepository<User>, IAlterTypeRepository<User>
    {
    }

    public interface IRepositoryBussines : IBaseRepository<Bussines>, IDeleteEditRepository<Bussines>, IDisableEnableRepository<Bussines>, IAlterTypeRepository<Bussines>
    {
    }
    public interface IRepositoryListCpf : IBaseRepository<ListCpf>, IDeleteEditRepository<ListCpf>, IAlterTypeRepository<ListCpf>
    {
        Task<IResultadoOperacao<dynamic>> CreateWithExcel((List<dynamic>, List<dynamic>) data);
    }
    
    public interface IRepositoryCard : IBaseRepository<Card>, IDeleteEditRepository<Card>, IAlterTypeRepository<Card>
    {
        Task<IResultadoOperacao<dynamic>> CreateWithExcel((List<dynamic>, List<dynamic>) data);
    }
    
    public interface IRepositoryBusStop : IBaseRepository<BusStop>, IDeleteEditRepository<BusStop>, IAlterTypeRepository<BusStop>, IDisableEnableRepository<BusStop>
    {
    }
}