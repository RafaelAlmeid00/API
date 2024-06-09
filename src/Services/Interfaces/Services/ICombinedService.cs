namespace Api.Interface;

public interface ICombinedService<T> : IBaseService<T>, IDeleteEditService<T>, ILoginLogoutService<T>, IAlterTypeService<T>, IDisableEnableService<T>
{
    Task<IResultadoOperacao<dynamic>> CreateWithExcel(IFormFile data);
}
