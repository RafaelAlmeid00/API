using Api.Domain;

namespace Api.Interface;

public interface IBussDTO
{
    int BusId { get; set; }
    string? BusNome { get; set; }
    string? BusPlaca { get; set; }
    string? BusFabricacao { get; set; }
    string? BusStatus { get; set; }
    string? BusModelo { get; set; }
    decimal BusTarifa { get; set; }
    string? BussinesBussCnpj { get; set; }
    string? BusRouteRouteNum { get; set; }
    BusRoute? BusRouteRouteNumNavigation { get; set; }
    Bussines? BussinesBussCnpjNavigation { get; set; }
    ICollection<TurnBus>? TurnBus { get; set; }
}