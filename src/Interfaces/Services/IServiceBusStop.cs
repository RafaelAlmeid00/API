using Api.Domain;

namespace Api.Interface;

public interface IServiceBusStop : IBaseService<BusStop>, IDeleteEditService<BusStop>, IAlterTypeService<BusStop>, IDisableEnableService<BusStop>
{
}