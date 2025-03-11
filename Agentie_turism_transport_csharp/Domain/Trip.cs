namespace DefaultNamespace;

public class Trip : Entity<long>
{
    public string TouristAttraction { get; set; }
    public string TransportCompany { get; set; }
    public TimeSpan DepartureTime { get; set; }
    public double Price { get; set; }
    public int AvailableSeats { get; set; }

    public Trip(string touristAttraction, string transportCompany, TimeSpan departureTime, double price, int availableSeats)
    {
        TouristAttraction = touristAttraction;
        TransportCompany = transportCompany;
        DepartureTime = departureTime;
        Price = price;
        AvailableSeats = availableSeats;
    }
}
