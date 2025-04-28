using System;

namespace networking.dto
{
    [Serializable]
    public class ReservationDTO
    {
        public long Id { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public int TicketCount { get; set; }
        public TripDTO Trip { get; set; }

        public ReservationDTO() { }

        public ReservationDTO(long id, string clientName, string clientPhone, int ticketCount, TripDTO trip)
        {
            Id = id;
            ClientName = clientName;
            ClientPhone = clientPhone;
            TicketCount = ticketCount;
            Trip = trip;
        }

        public override string ToString()
        {
            return $"{ClientName} ({ClientPhone}) - {TicketCount} tickets for Trip {Trip}";
        }
    }
}