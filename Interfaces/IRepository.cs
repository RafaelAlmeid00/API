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

    public interface ILoginLogoutRepository<T>
    {
        Task<IResultadoOperacao<T>> Login(T data);
        Task<IResultadoOperacao<T>> Logout(T data);
    }

    public interface IRepository<T> : IBaseRepository<T>, IDeleteEditRepository<T>, IAlterTypeRepository<T>, IDisableEnableRepository<T>, ILoginLogoutRepository<T>
    {
    }



}