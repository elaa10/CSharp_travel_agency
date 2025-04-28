

using Agentie_turism_transport_csharp.networking;
using model;
using networking.dto;

namespace networking;

public static class JsonProtocolUtils
{
    public static Response CreateOkResponse(object data = null)
    {
        return new Response { Type = ResponseType.OK};
    }

    public static Response CreateErrorResponse(string errorMessage)
    {
        return new Response { Type = ResponseType.ERROR, Data = errorMessage };
    }

    public static Response CreateGetAllTripsResponse(List<Trip> trips)
    {
        return new Response { Type = ResponseType.GET_ALL_TRIPS, Trips = DTOUtils.GetDTO(trips.ToArray()) };
    }

    public static Response CreateGetAllTripsByDateResponse(List<Trip> trips)
    {
        return new Response { Type = ResponseType.GET_ALL_TRIPS_BY_DATE, Trips = DTOUtils.GetDTO(trips.ToArray())};
    }

    public static Response CreateFindTripResponse(Trip trip)
    {
        return new Response { Type = ResponseType.FIND_TRIP, Trip = DTOUtils.GetDTO(trip) };
    }

    public static Response CreateReservationMadeResponse(Reservation reservation)
    {
        return new Response { Type = ResponseType.RESERVATION_MADE, Reservation = DTOUtils.GetDTO(reservation) };
    }
    
    
    
    public static Request CreateLoginRequest(string[] credentials)
    {
        return new Request { Type = RequestType.LOGIN, Data = credentials };
    }

    public static Request CreateLogoutRequest(SoftUser softUser)
    {
        return new Request { Type = RequestType.LOGOUT, User = DTOUtils.GetDTO(softUser) };
    }

    public static Request CreateGetAllTripsRequest()
    {
        return new Request { Type = RequestType.GET_ALL_TRIPS};
    }

    public static Request CreateGetTripsByDateRequest(string[] data)
    {
        return new Request { Type = RequestType.GET_ALL_TRIPS_BY_DATE, Data = data };
    }

    public static Request CreateFindTripRequest(long id)
    {
        return new Request { Type = RequestType.FIND_TRIP, Data = id };
    }

    public static Request CreateMakeReservationRequest(ReservationDTO reservation)
    {
        return new Request { Type = RequestType.MAKE_RESERVATION, Reservation = reservation };
    }
}