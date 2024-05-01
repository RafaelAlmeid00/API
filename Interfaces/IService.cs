using Api.Domain;
using NuGet.Common;

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
        Task<IResultadoOperacao<object>> Login(T data);
    }

    public interface IServiceAdmin<T> : IBaseService<T>, IDeleteEditService<T>, ILoginLogoutService<IAdminLoginDTO>
    {
    }

    public interface IServiceUser<T> : IBaseService<T>, IDeleteEditService<T>, ILoginLogoutService<IUserLoginDTO>, IAlterTypeService<T>, IDisableEnableService<T>
    {
    }

}