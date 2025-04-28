
using System.Collections.Generic;
using System.Data;
using log4net;
using model;
using persistence.interfaces;

namespace persistence
{
    public class TripDBRepository : ITripRepository
    {
        private readonly IDictionary<string, string> props;
        private static readonly ILog log = LogManager.GetLogger(typeof(TripDBRepository));

        public TripDBRepository(IDictionary<string, string> props)
        {
            log.DebugFormat("Initializing TripDBRepository with properties: {0}", props);
            this.props = props;
        }

        public Trip FindOne(long id)
        {
            log.DebugFormat("Finding trip with ID {0}", id);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM trips WHERE id = @id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var trip = ExtractTripFromReader(reader);
                        log.DebugFormat("Found trip: {0}", trip);
                        return trip;
                    }
                }
            }
            return null;
        }

        public IEnumerable<Trip> FindAll()
        {
            log.Debug("Finding all trips");
            var trips = new List<Trip>();
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM trips";
                
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var trip = ExtractTripFromReader(reader);
                        trips.Add(trip);
                    }
                }
            }
            return trips;
        }

        public bool Save(Trip trip)
        {
            log.DebugFormat("Saving trip {0}", trip);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = @"INSERT INTO trips (touristAttraction, transportCompany, departureTime, price, availableSeats) 
                                    VALUES (@touristAttraction, @transportCompany, @departureTime, @price, @availableSeats);
                                    SELECT last_insert_rowid();";

                var paramAttraction = comm.CreateParameter();
                paramAttraction.ParameterName = "@touristAttraction";
                paramAttraction.Value = trip.touristAttraction;
                comm.Parameters.Add(paramAttraction);

                var paramCompany = comm.CreateParameter();
                paramCompany.ParameterName = "@transportCompany";
                paramCompany.Value = trip.transportCompany;
                comm.Parameters.Add(paramCompany);

                var paramTime = comm.CreateParameter();
                paramTime.ParameterName = "@departureTime";
                paramTime.Value = trip.departureTime.ToString("yyyy-MM-dd HH:mm:ss");
                comm.Parameters.Add(paramTime);

                var paramPrice = comm.CreateParameter();
                paramPrice.ParameterName = "@price";
                paramPrice.Value = trip.price;
                comm.Parameters.Add(paramPrice);

                var paramSeats = comm.CreateParameter();
                paramSeats.ParameterName = "@availableSeats";
                paramSeats.Value = trip.availableSeats;
                comm.Parameters.Add(paramSeats);

                var generatedId = comm.ExecuteScalar();
                if (generatedId != null)
                {
                    trip.Id = (long)generatedId;
                    return true;
                }
            }
            return false;
        }

        public bool Delete(long id)
        {
            log.DebugFormat("Deleting trip with id {0}", id);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "DELETE FROM trips WHERE id = @id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                int rowsDeleted = comm.ExecuteNonQuery();
                return rowsDeleted > 0;
            }
        }

        public bool Update(Trip trip)
        {
            log.DebugFormat("Updating trip {0}", trip);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = @"UPDATE trips SET 
                                    touristAttraction = @touristAttraction, 
                                    transportCompany = @transportCompany, 
                                    departureTime = @departureTime, 
                                    price = @price, 
                                    availableSeats = @availableSeats 
                                    WHERE id = @id";

                var paramAttraction = comm.CreateParameter();
                paramAttraction.ParameterName = "@touristAttraction";
                paramAttraction.Value = trip.touristAttraction;
                comm.Parameters.Add(paramAttraction);

                var paramCompany = comm.CreateParameter();
                paramCompany.ParameterName = "@transportCompany";
                paramCompany.Value = trip.transportCompany;
                comm.Parameters.Add(paramCompany);

                var paramTime = comm.CreateParameter();
                paramTime.ParameterName = "@departureTime";
                paramTime.Value = trip.departureTime.ToString("yyyy-MM-dd HH:mm:ss");
                comm.Parameters.Add(paramTime);

                var paramPrice = comm.CreateParameter();
                paramPrice.ParameterName = "@price";
                paramPrice.Value = trip.price;
                comm.Parameters.Add(paramPrice);

                var paramSeats = comm.CreateParameter();
                paramSeats.ParameterName = "@availableSeats";
                paramSeats.Value = trip.availableSeats;
                comm.Parameters.Add(paramSeats);

                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = trip.Id;
                comm.Parameters.Add(paramId);

                int rowsUpdated = comm.ExecuteNonQuery();
                return rowsUpdated > 0;
            }
        }

        public IEnumerable<Trip> FindTripsByObjectiveDateAndTimeRange(string objective, DateTime departureDate, int startHour, int endHour)
        {
            log.DebugFormat("Finding trips for objective '{0}' on {1} between {2} and {3}", 
                objective, departureDate, startHour, endHour);
            
            var trips = new List<Trip>();
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = @"SELECT * FROM trips 
                                    WHERE touristAttraction = @objective 
                                    AND date(departureTime) = @departureDate 
                                    AND strftime('%H', departureTime) BETWEEN @startHour AND @endHour";

                var paramObjective = comm.CreateParameter();
                paramObjective.ParameterName = "@objective";
                paramObjective.Value = objective;
                comm.Parameters.Add(paramObjective);

                var paramDate = comm.CreateParameter();
                paramDate.ParameterName = "@departureDate";
                paramDate.Value = departureDate.Date.ToString("yyyy-MM-dd");
                comm.Parameters.Add(paramDate);

                var paramStart = comm.CreateParameter();
                paramStart.ParameterName = "@startHour";
                paramStart.Value = startHour.ToString("00");
                comm.Parameters.Add(paramStart);

                var paramEnd = comm.CreateParameter();
                paramEnd.ParameterName = "@endHour";
                paramEnd.Value = endHour.ToString("00");
                comm.Parameters.Add(paramEnd);

                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var trip = ExtractTripFromReader(reader);
                        trips.Add(trip);
                    }
                }
            }
            return trips;
        }

        private Trip ExtractTripFromReader(IDataReader reader)
        {
            var departureTime = DateTime.Parse(reader["departureTime"].ToString());
            var trip = new Trip(
                reader["touristAttraction"].ToString(),
                reader["transportCompany"].ToString(),
                departureTime,
                Convert.ToDouble(reader["price"]),
                Convert.ToInt32(reader["availableSeats"]))
            {
                Id = Convert.ToInt64(reader["id"])
            };
            return trip;
        }
    }
}