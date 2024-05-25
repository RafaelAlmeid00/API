using Api.Domain;
using Microsoft.CodeAnalysis.Operations;

namespace Api.Interface;

public interface IRepositoryBussines : IBaseRepository<Bussines>, IDeleteEditRepository<Bussines>, IDisableEnableRepository<Bussines>, IAlterTypeRepository<Bussines>
    {
    }