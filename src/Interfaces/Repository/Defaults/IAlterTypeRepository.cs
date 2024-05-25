namespace Api.Interface
{
    public interface IAlterTypeRepository<T>
    {
        Task<IResultadoOperacao<T>> AlterType(T data);
    }
}