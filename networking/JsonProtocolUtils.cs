

using Agentie_turism_transport_csharp.networking;
using model;
using networking.dto;

namespace networking;

public static class JsonProtocolUtils
{
    public static Response CreateOkResponse()
    {
        return new Response { Type = ResponseType.OK};
    }

    public static Response CreateErrorResponse(string errorMessage)
    {
        return new Response { Type = ResponseType.ERROR, ErrorMessage = errorMessage };
    }

    public static Response CreateGetAllTripsResponse(List<Trip> trips)
    {
        return new Response { Type = ResponseType.GET_ALL_TRIPS, Trips = trips.ToArray()};
    }

    public static Response CreateGetAllTripsByDateResponse(List<Trip> trips)
    {
        return new Response { Type = ResponseType.GET_ALL_TRIPS_BY_DATE, Trips = trips.ToArray()};
    }

    public static Response CreateFindTripResponse(Trip trip)
    {
        return new Response { Type = ResponseType.FIND_TRIP, Trip = trip };
    }

    public static Response CreateReservationMadeResponse(Reservation reservation)
    {
        return new Response { Type = ResponseType.RESERVATION_MADE, Reservation = reservation };
    }
    
    
    
    public static Request CreateLoginRequest(SoftUser user)
    {
        return new Request { Type = RequestType.LOGIN, User = user };
    }

    public static Request CreateLogoutRequest(SoftUser softUser)
    {
        return new Request { Type = RequestType.LOGOUT, User = softUser };
    }

    public static Request CreateGetAllTripsRequest()
    {
        return new Request { Type = RequestType.GET_ALL_TRIPS};
    }

    public static Request CreateGetTripsByDateRequest(SearchTripDTO searchTripDTO)
    {
        return new Request { Type = RequestType.GET_ALL_TRIPS_BY_DATE, SearchTrip = searchTripDTO };
    }

    public static Request CreateFindTripRequest(long id)
    {
        return new Request { Type = RequestType.FIND_TRIP, TripId = id };
    }

    public static Request CreateMakeReservationRequest(Reservation reservation)
    {
        return new Request { Type = RequestType.MAKE_RESERVATION, Reservation = reservation };
    }
}