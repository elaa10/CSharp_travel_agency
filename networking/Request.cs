using Agentie_turism_transport_csharp.networking;
using model;
using networking.dto;

namespace networking;

public class Request
{
    public RequestType Type { get; set; }
    public SoftUser User { get; set; }
    
    public Trip Trip { get; set; }
    
    public long TripId { get; set; }
    
    public Reservation Reservation { get; set; }
    
    public SearchTripDTO SearchTrip { get; set; }
    
    
    
}