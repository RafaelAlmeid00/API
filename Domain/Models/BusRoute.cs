using System;
using System.Collections.Generic;

namespace Api.Domain;

public record BusRoute
{
    public string RouteNum { get; set; } = null!;

    public string RouteNome { get; set; } = null!;

    public virtual ICollection<Buss> Busses { get; set; } = [];

    public virtual ICollection<BusStop> BusStopStops { get; set; } = [];
}
