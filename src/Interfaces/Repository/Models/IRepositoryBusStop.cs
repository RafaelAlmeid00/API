using Api.Domain;

namespace Api.Interface;
public interface IRepositoryBusStop : IBaseRepository<BusStop>, IDeleteEditRepository<BusStop>, IAlterTypeRepository<BusStop>, IDisableEnableRepository<BusStop>
{
}