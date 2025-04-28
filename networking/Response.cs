using Agentie_turism_transport_csharp.networking;
using model;
using networking.dto;

namespace networking;

public class Response
{
    public ResponseType Type { get; set; }
    public string ErrorMessage { get; set; }
  
    public Trip Trip { get; set; }
    
    public Trip[] Trips { get; set; }
    
    public Reservation Reservation { get; set; }
    
    public SearchTripDTO SearchTrip { get; set; }
    
    public Response()
    {
    }
    public override string ToString()
    {
        return $"[type={Type}, error={ErrorMessage}]";
    }
}