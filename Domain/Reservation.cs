namespace TravelAgency.Domain
{
    public class Reservation
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public int TicketCount { get; set; }
        public Trip Trip { get; set; }

        public Reservation(int id, string clientName, string clientPhone, int ticketCount, Trip trip)
        {
            Id = id;
            ClientName = clientName;
            ClientPhone = clientPhone;
            TicketCount = ticketCount;
            Trip = trip;
        }
    }
} 