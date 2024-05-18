using System;
using System.Collections.Generic;

namespace Api.Domain;

public record BusStop
{
    public int StopId { get; set; }

    public string StopEndCep { get; set; } = null!;

    public string StopEndUf { get; set; } = null!;

    public string StopEndrua { get; set; } = null!;

    public string StopEndnum { get; set; } = null!;

    public string? StopEndcomplemento { get; set; }

    public string StopEndcidade { get; set; } = null!;

    public virtual ICollection<BusRoute> BusRouteRouteNums { get; set; } = [];
}
