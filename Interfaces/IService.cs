using Api.Domain;

namespace Api.Interface
{

    public interface IBaseService<T>
    {
        Task<IResultadoOperacao<List<T>>> Search(T data);
        Task<IResultadoOperacao<T>> GetOne(T data);
        Task<IResultadoOperacao<T>> Create(T data);
    }
    public interface IDeleteEditService<T>
    {
        Task<IResultadoOperacao<T>> Delete(T data);
        Task<IResultadoOperacao<T>> Edit(T data);
    }
    public interface IAlterTypeService<T>
    {
        Task<IResultadoOperacao<T>> AlterType(T data);
    }

    public interface IDisableEnableService<T>
    {
        Task<IResultadoOperacao<T>> Disable(T data);
        Task<IResultadoOperacao<T>> Enable(T data);
    }

    public interface ILoginLogoutService<T>
    {
        Task<IResultadoOperacao<string>> Login(T data);
        Task<IResultadoOperacao<string>> Logout(T data);
    }

    public interface IService<T> : IBaseService<T>, IAlterTypeService<T>, IDeleteEditService<T>, IDisableEnableService<T>, ILoginLogoutService<T>
    {
    }


}