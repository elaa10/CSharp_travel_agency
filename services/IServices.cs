using model;
using services;

namespace services;
    
public interface IServices
{
    // SoftUser
    SoftUser Login(string username, string password, IObserver softUserObserver);
    void Logout(SoftUser softUser, IObserver softUserObserver);

    // Trip
    IEnumerable<Trip> GetAllTrips();
    IEnumerable<Trip> SearchTripsByObjectiveAndTime(string objective, DateTime date, int startHour, int endHour);
    Trip GetTripById(long id);

    // Reservation
    void MakeReservation(string clientName, string clientPhone, int ticketCount, Trip trip);

}