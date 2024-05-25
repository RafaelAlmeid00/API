using Api.Domain;

namespace Api.Interface;

public interface IRepositoryAdmin : IBaseRepository<Admin>, IDeleteEditRepository<Admin>
{
}