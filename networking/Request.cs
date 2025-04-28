using Agentie_turism_transport_csharp.networking;
using model;
using networking.dto;

namespace networking;

public class Request
{
    public RequestType Type { get; set; }
    public object Data { get; set; }
    
    public SoftUserDTO User { get; set; }
    
    public TripDTO Trip { get; set; }
    
    public ReservationDTO Reservation { get; set; }
    
    public SearchTripDTO SearchTrip { get; set; }
    
    
}