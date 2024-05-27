using Api.Domain;
using NuGet.Common;

namespace Api.Interface;

public interface IBaseService<T>
{
    Task<IResultadoOperacao<List<T>>> Search(T data);
    Task<IResultadoOperacao<T>> GetOne(T data);
    Task<IResultadoOperacao<T>> Create(T data);
}

