using log4net;
using model;
using System.Collections.Generic;
using System.Data;
using model;
using persistence.interfaces;

namespace persistence
{
    public class ReservationDBRepository : IReservationRepository
    {
        private readonly IDictionary<string, string> props;
        private static readonly ILog log = LogManager.GetLogger(typeof(ReservationDBRepository));
        private readonly IRepository<long, Trip> tripRepository;

        public ReservationDBRepository(IDictionary<string, string> props, IRepository<long, Trip> tripRepository)
        {
            log.DebugFormat("Initializing ReservationDBRepository with properties: {0}", props);
            this.props = props;
            this.tripRepository = tripRepository;
        }

        public Reservation FindOne(long id)
        {
            log.DebugFormat("Finding reservation with ID {0}", id);
            var con = DBUtils.GetConnection(props);
            
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM reservations WHERE id = @id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tripId = (long)reader["trip"];
                        var trip = tripRepository.FindOne(tripId);
                        var reservation = new Reservation(
                            (string)reader["clientName"],
                            (string)reader["clientPhone"],
                            (int)reader["ticketCount"],
                            trip)
                        {
                            Id = (long)reader["id"]
                        };
                        return reservation;
                    }
                }
            }
            return null;
        }

        public IEnumerable<Reservation> FindAll()
        {
            log.Debug("Finding all reservations");
            var reservations = new List<Reservation>();
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM reservations";
                
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tripId = (long)reader["trip"];
                        var trip = tripRepository.FindOne(tripId);
                        var reservation = new Reservation(
                            (string)reader["clientName"],
                            (string)reader["clientPhone"],
                            (int)reader["ticketCount"],
                            trip)
                        {
                            Id = (long)reader["id"]
                        };
                        reservations.Add(reservation);
                    }
                }
            }
            return reservations;
        }

        public bool Save(Reservation reservation)
        {
            log.DebugFormat("Saving reservation {0}", reservation);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = @"INSERT INTO reservations (clientName, clientPhone, ticketCount, trip) 
                                    VALUES (@clientName, @clientPhone, @ticketCount, @trip);
                                    SELECT last_insert_rowid();";

                var paramClientName = comm.CreateParameter();
                paramClientName.ParameterName = "@clientName";
                paramClientName.Value = reservation.clientName;
                comm.Parameters.Add(paramClientName);

                var paramClientPhone = comm.CreateParameter();
                paramClientPhone.ParameterName = "@clientPhone";
                paramClientPhone.Value = reservation.clientPhone;
                comm.Parameters.Add(paramClientPhone);

                var paramTicketCount = comm.CreateParameter();
                paramTicketCount.ParameterName = "@ticketCount";
                paramTicketCount.Value = reservation.ticketCount;
                comm.Parameters.Add(paramTicketCount);

                var paramTrip = comm.CreateParameter();
                paramTrip.ParameterName = "@trip";
                paramTrip.Value = reservation.trip.Id;
                comm.Parameters.Add(paramTrip);

                var generatedId = comm.ExecuteScalar();
                if (generatedId != null)
                {
                    reservation.Id = (long)generatedId;
                    return true;
                }
            }
            return false;
        }

        public bool Delete(long id)
        {
            log.DebugFormat("Deleting reservation with id {0}", id);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "DELETE FROM reservations WHERE id = @id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                int rowsDeleted = comm.ExecuteNonQuery();
                return rowsDeleted > 0;
            }
        }

        public bool Update(Reservation reservation)
        {
            log.DebugFormat("Updating reservation {0}", reservation);
            var con = DBUtils.GetConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = @"UPDATE reservations SET 
                                    clientName = @clientName, 
                                    clientPhone = @clientPhone, 
                                    ticketCount = @ticketCount, 
                                    trip = @trip 
                                    WHERE id = @id";

                var paramClientName = comm.CreateParameter();
                paramClientName.ParameterName = "@clientName";
                paramClientName.Value = reservation.clientName;
                comm.Parameters.Add(paramClientName);

                var paramClientPhone = comm.CreateParameter();
                paramClientPhone.ParameterName = "@clientPhone";
                paramClientPhone.Value = reservation.clientPhone;
                comm.Parameters.Add(paramClientPhone);

                var paramTicketCount = comm.CreateParameter();
                paramTicketCount.ParameterName = "@ticketCount";
                paramTicketCount.Value = reservation.ticketCount;
                comm.Parameters.Add(paramTicketCount);

                var paramTrip = comm.CreateParameter();
                paramTrip.ParameterName = "@trip";
                paramTrip.Value = reservation.trip.Id;
                comm.Parameters.Add(paramTrip);

                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = reservation.Id;
                comm.Parameters.Add(paramId);

                int rowsUpdated = comm.ExecuteNonQuery();
                return rowsUpdated > 0;
            }
        }
    }
}