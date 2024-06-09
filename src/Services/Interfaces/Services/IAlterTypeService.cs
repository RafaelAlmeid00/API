namespace Api.Interface;

public interface IAlterTypeService<T>
{
    Task<IResultadoOperacao<T>> AlterType(T data);
}