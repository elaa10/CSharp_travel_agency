using model;

namespace services;

public interface IObserver
{
    void ReservationMade(Reservation reservation);
}