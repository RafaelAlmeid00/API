
using Api.Interface;

namespace Api.Domain;

public record BusStop : IBussStopDTO
{
    public int StopId { get; set; }
    public string? StopEndCep { get; set; }
    public string? StopEndUf { get; set; }
    public string? StopEndrua { get; set; }
    public string? StopEndnum { get; set; }
    public string?  StopEndcomplemento { get; set; }
    public string? StopEndcidade { get; set; }
    public virtual ICollection<BusRoute>? BusRouteRouteNums { get; set; } = [];
}
