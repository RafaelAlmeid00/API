using Api.Domain;

namespace Api.Interface;

public interface IServiceCard : IBaseService<Card>, IDeleteEditService<Card>, IAlterTypeService<Card>
{
}