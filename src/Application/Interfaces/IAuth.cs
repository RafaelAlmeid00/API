
namespace Api.Interface
{
    public interface IAuth
    {
        string CreateTokenAdmin(IAdminDTO data);
        string CreateTokenUser(IUserDTO data);
        string CreateTokenBussines(IBussinesDTO data);
    }

}