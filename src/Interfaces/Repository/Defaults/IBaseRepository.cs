namespace Api.Interface
{
    public interface IBaseRepository<T>
    {
        Task<IResultadoOperacao<List<T>>> Search(T data);
        Task<IResultadoOperacao<T>> GetOne(T data);
        Task<IResultadoOperacao<T>> Create(T data);
    }
}