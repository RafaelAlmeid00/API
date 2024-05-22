using Api.Domain;

namespace Api.Interface;

public interface IServiceBussines : IBaseService<Bussines>, IDeleteEditService<Bussines>, IAlterTypeService<Bussines>
{
}