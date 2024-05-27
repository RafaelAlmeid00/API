using Api.Domain;

namespace Api.Interface;

public interface IServiceAdmin : IBaseService<Admin>, IDeleteEditService<Admin>, ILoginLogoutService<IAdminLoginDTO>
{
}