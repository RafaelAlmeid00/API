using Api.Domain;
using Microsoft.CodeAnalysis.Operations;

namespace Api.Interface;

public interface IServiceBussines : IBaseService<Bussines>, IDeleteEditService<Bussines>, ILoginLogoutService<IBussinesLoginDTO>, IAlterTypeService<Bussines>, IDisableEnableService<Bussines>
{
}