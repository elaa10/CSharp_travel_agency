using System;

namespace model
{
    public class Trip : Entity<long>
    {
        public string touristAttraction { get; set; }
        public string transportCompany { get; set; }
        public DateTime departureTime { get; set; }
        public double price { get; set; }
        public int availableSeats { get; set; }

        public Trip(string touristAttraction, string transportCompany, DateTime departureTime, double price, int availableSeats)
        {
            this.touristAttraction = touristAttraction;
            this.transportCompany = transportCompany;
            this.departureTime = departureTime;
            this.price = price;
            this.availableSeats = availableSeats;
        }

        public override string ToString()
        {
            return $"{touristAttraction} {transportCompany} {departureTime} {price} {availableSeats} {Id}";
        }
    }
}