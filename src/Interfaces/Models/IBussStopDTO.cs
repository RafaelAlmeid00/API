using Api.Domain;

namespace Api.Interface
{
    public interface IBussStopDTO
    {
        int StopId { get; set; }
        string? StopEndCep { get; set; }
        string? StopEndUf { get; set; }
        string? StopEndrua { get; set; }
        string? StopEndnum { get; set; }
        string? StopEndcomplemento { get; set; }
        string? StopEndcidade { get; set; }
        ICollection<BusRoute>? BusRouteRouteNums { get; set; }
    }
}