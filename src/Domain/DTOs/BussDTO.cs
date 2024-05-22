using Api.Domain;

namespace Api.Interface;

public class BussDTO
{
    public int BusId { get; set; }
    public string? BusNome { get; set; }
    public string? BusPlaca { get; set; }
    public string? BusFabricacao { get; set; }
    public string? BusStatus { get; set; }
    public string? BusModelo { get; set; }
    public decimal BusTarifa { get; set; }
    public string? BussinesBussCnpj { get; set; }
    public string? BusRouteRouteNum { get; set; }
    public virtual BusRoute? BusRouteRouteNumNavigation { get; set; }
    public virtual Bussines? BussinesBussCnpjNavigation { get; set; }
    public virtual ICollection<TurnBus>? TurnBus { get; set; }
}