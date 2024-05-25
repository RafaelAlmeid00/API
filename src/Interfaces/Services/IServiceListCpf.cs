using Api.Domain;

namespace Api.Interface;

public interface IServiceListCpf : IBaseService<ListCpf>, IDeleteEditService<ListCpf>, IAlterTypeService<ListCpf>
{
    Task<IResultadoOperacao<dynamic>> CreateWithExcel(IFormFile data);
}

