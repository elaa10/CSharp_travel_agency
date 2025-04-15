using System;

namespace TravelAgency.Domain
{
    public class Trip
    {
        public int Id { get; set; }
        public string TouristAttraction { get; set; }
        public string TransportCompany { get; set; }
        public DateTime DepartureTime { get; set; }
        public double Price { get; set; }
        public int AvailableSeats { get; set; }

        public Trip(int id, string touristAttraction, string transportCompany, 
                   DateTime departureTime, double price, int availableSeats)
        {
            Id = id;
            TouristAttraction = touristAttraction;
            TransportCompany = transportCompany;
            DepartureTime = departureTime;
            Price = price;
            AvailableSeats = availableSeats;
        }
    }
} 