using System;

namespace networking.dto
{
    [Serializable]
    public class TripDTO
    {
        public long Id { get; set; }
        public string TouristAttraction { get; set; }
        public string TransportCompany { get; set; }
        public DateTime DepartureTime { get; set; }
        public double Price { get; set; }
        public int AvailableSeats { get; set; }

        public TripDTO() { }

        public TripDTO(long id, string touristAttraction, string transportCompany, DateTime departureTime, double price, int availableSeats)
        {
            Id = id;
            TouristAttraction = touristAttraction;
            TransportCompany = transportCompany;
            DepartureTime = departureTime;
            Price = price;
            AvailableSeats = availableSeats;
        }

        public override string ToString()
        {
            return $"{TouristAttraction} - {TransportCompany} - {DepartureTime} - {Price}€ - Seats:{AvailableSeats}";
        }
    }
}