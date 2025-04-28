using model;

namespace networking.dto
{
    public class DTOUtils
    {
        public static SoftUser GetFromDTO(SoftUserDTO dto)
        {
            SoftUser user = new SoftUser(dto.Username, dto.Password);
            user.Id = dto.Id;
            return user;
        }

        public static SoftUserDTO GetDTO(SoftUser user)
        {
            return new SoftUserDTO(user.Id, user.username, user.password);
        }

        public static Trip GetFromDTO(TripDTO dto)
        {
            Trip trip = new Trip(dto.TouristAttraction, dto.TransportCompany, dto.DepartureTime, dto.Price, dto.AvailableSeats);
            trip.Id = dto.Id;
            return trip;
        }

        public static TripDTO GetDTO(Trip trip)
        {
            return new TripDTO(trip.Id, trip.touristAttraction, trip.transportCompany, trip.departureTime, trip.price, trip.availableSeats);
        }

        public static Reservation GetFromDTO(ReservationDTO dto)
        {
            Reservation reservation = new Reservation(dto.ClientName, dto.ClientPhone, dto.TicketCount, GetFromDTO(dto.Trip));
            reservation.Id = dto.Id;
            return reservation;
        }

        public static ReservationDTO GetDTO(Reservation reservation)
        {
            return new ReservationDTO(reservation.Id, reservation.clientName, reservation.clientPhone, reservation.ticketCount, GetDTO(reservation.trip));
        }

        public static TripDTO[] GetDTO(Trip[] trips)
        {
            TripDTO[] tripsDTO = new TripDTO[trips.Length];
            for (int i = 0; i < trips.Length; i++)
                tripsDTO[i] = GetDTO(trips[i]);
            return tripsDTO;
        }

        public static Trip[] GetFromDTO(TripDTO[] tripsDTO)
        {
            Trip[] trips = new Trip[tripsDTO.Length];
            for (int i = 0; i < tripsDTO.Length; i++)
                trips[i] = GetFromDTO(tripsDTO[i]);
            return trips;
        }
        
        public static SearchTripDTO GetFromDTO(SearchTripDTO dto)
        {
            return new SearchTripDTO(dto.Objective, dto.Date, dto.StartHour, dto.EndHour);
        }

        public static SearchTripDTO GetDTO(SearchTripDTO searchTrip)
        {
            return new SearchTripDTO(searchTrip.Objective, searchTrip.Date, searchTrip.StartHour, searchTrip.EndHour);
        }

    }
}