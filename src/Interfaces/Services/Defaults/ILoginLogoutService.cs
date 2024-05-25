namespace Api.Interface;

public interface ILoginLogoutService<T>
{
    Task<IResultadoOperacao<object>> Login(T data);
}