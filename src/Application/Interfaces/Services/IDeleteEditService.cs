namespace Api.Interface;

public interface IDeleteEditService<T>
{
    Task<IResultadoOperacao<T>> Delete(T data);
    Task<IResultadoOperacao<T>> Edit(T data);
}