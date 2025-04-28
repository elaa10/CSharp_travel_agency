using model;

namespace persistence.interfaces;

public interface ITripRepository : IRepository<long, Trip>
{
    public IEnumerable<Trip> FindTripsByObjectiveDateAndTimeRange(string objective, DateTime departureDate, int startHour, int endHour);
}