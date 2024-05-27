using Api.Domain;

namespace Api.Interface;

public interface IServiceUser : IBaseService<User>, IDeleteEditService<User>, ILoginLogoutService<IUserLoginDTO>, IAlterTypeService<User>, IDisableEnableService<User>
{
}