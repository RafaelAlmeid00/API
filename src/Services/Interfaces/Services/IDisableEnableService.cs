namespace Api.Interface;

public interface IDisableEnableService<T>
{
    Task<IResultadoOperacao<T>> Disable(T data);
    Task<IResultadoOperacao<T>> Enable(T data);
}