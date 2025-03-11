namespace DefaultNamespace;

public class Reservation : Entity<long>
{
    public string ClientName { get; set; }
    public string ClientPhone { get; set; }
    public int TicketCount { get; set; }
    public Trip Trip { get; set; }

    public Reservation(string clientName, string clientPhone, int ticketCount, Trip trip)
    {
        ClientName = clientName;
        ClientPhone = clientPhone;
        TicketCount = ticketCount;
        Trip = trip;
    }
}
