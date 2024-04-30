using System;
using System.Collections.Generic;

namespace Api.Domain;

public partial class Buss
{
    public int BusId { get; set; }

    public string BusNome { get; set; } = null!;

    public string BusPlaca { get; set; } = null!;

    public string BusFabricacao { get; set; } = null!;

    public string BusStatus { get; set; } = null!;

    public string BusModelo { get; set; } = null!;

    public decimal BusTarifa { get; set; }

    public string BussinesBussCnpj { get; set; } = null!;

    public string BusRouteRouteNum { get; set; } = null!;

    public virtual BusRoute BusRouteRouteNumNavigation { get; set; } = null!;

    public virtual Bussines BussinesBussCnpjNavigation { get; set; } = null!;

    public virtual ICollection<TurnBus> TurnBus { get; set; } = [];
}
