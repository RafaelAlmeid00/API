namespace Api.Interface
{

    public interface IDisableEnableRepository<T>
    {
        Task<IResultadoOperacao<T>> Disable(T data);
        Task<IResultadoOperacao<T>> Enable(T data);
    }

}