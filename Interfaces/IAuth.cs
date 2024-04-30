
namespace Api.Interface
{
    public interface IAuth<T>
    {
        string CreateToken(T data);
    }
}