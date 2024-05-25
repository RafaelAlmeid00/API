using Api.Domain;

namespace Api.Interface;


    public interface IRepositoryUser : IBaseRepository<User>, IDeleteEditRepository<User>, IDisableEnableRepository<User>, IAlterTypeRepository<User>
    {
    }
