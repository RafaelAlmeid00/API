namespace Api.Interface
{
    public interface IDeleteEditRepository<T>
    {
        Task<IResultadoOperacao<T>> Delete(T data);
        Task<IResultadoOperacao<T>> Edit(T data);
    }
}