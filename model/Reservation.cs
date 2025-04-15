using System;
using model;

namespace model
{
    public class Reservation : Entity<long>
    {
        public string clientName { get; set; }
        public string clientPhone { get; set; }
        public int ticketCount { get; set; }
        public Trip trip { get; set; }

        public Reservation(string clientName, string clientPhone, int ticketCount, Trip trip)
        {
            this.clientName = clientName;
            this.clientPhone = clientPhone;
            this.ticketCount = ticketCount;
            this.trip = trip;
        }

        public override string ToString()
        {
            return $"{clientName}, {clientPhone}, {ticketCount}, {trip?.Id}";
        }
    }
}