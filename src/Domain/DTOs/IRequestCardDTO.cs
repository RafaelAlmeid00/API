namespace Api.Interface;

public class IRequestCardDTO
{
    public int ReqId { get; set; }

    public DateOnly ReqData { get; set; }

    public DateOnly? ReqEnvio { get; set; }

    public string ReqTipoCartao { get; set; } = null!;

    public string UserUserCpf { get; set; } = null!;
}