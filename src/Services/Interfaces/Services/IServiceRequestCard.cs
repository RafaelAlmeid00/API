using Api.Domain;

namespace Api.Interface;

public interface IServiceRequestCard : IBaseService<RequestCard>, IDeleteEditService<RequestCard>, IAlterTypeService<RequestCard>
{
    
}