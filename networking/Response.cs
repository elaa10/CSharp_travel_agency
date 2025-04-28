using Agentie_turism_transport_csharp.networking;
using networking.dto;

namespace networking;

public class Response
{
    public ResponseType Type { get; set; }
    public string ErrorMessage { get; set; }
    public object Data { get; set; } // Poate fi orice obiect din domeniu

    public SoftUserDTO User { get; set; }

    public TripDTO Trip { get; set; }
    
    public TripDTO[] Trips { get; set; }
    
    public ReservationDTO Reservation { get; set; }
    
    public SearchTripDTO SearchTrip { get; set; }
    
    public override string ToString()
    {
        return $"[type={Type}, error={ErrorMessage}, data={Data}]";
    }
}