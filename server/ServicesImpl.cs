using System.Collections.Concurrent;
using log4net;
using model;
using persistence.interfaces;
using services;

namespace server;

public class ServicesImpl : IServices
    {
        private readonly ISoftUserRepository softUserRepo;
        private readonly ITripRepository tripRepo;
        private readonly IReservationRepository reservationRepo;
        private readonly ConcurrentDictionary<long, IObserver> loggedSoftUsers;

        private static readonly ILog logger = LogManager.GetLogger(typeof(ServicesImpl));

        public ServicesImpl(ISoftUserRepository softUserRepository, ITripRepository tripRepository, IReservationRepository reservationRepository)
        {
            softUserRepo = softUserRepository;
            tripRepo = tripRepository;
            reservationRepo = reservationRepository;
            loggedSoftUsers = new ConcurrentDictionary<long, IObserver>();
        }

        // -------------------- SOFT USER

        public SoftUser Login(string username, string password, IObserver softUserObserver)
        {
            lock (this)
            {
                var loginUser = softUserRepo.FindByUsernameAndPassword(username, password);

                if (loginUser != null)
                {
                    if (loggedSoftUsers.ContainsKey(loginUser.Id))
                    {
                        throw new MyException("User already logged in.");
                    }
                    loggedSoftUsers.TryAdd(loginUser.Id, softUserObserver);
                    logger.Info("User logged in: " + loginUser.username);
                    return loginUser;
                }
                else
                {
                    throw new MyException("Authentication failed.");
                }
            }
        }

        public void Logout(SoftUser softUser, IObserver softUserObserver)
        {
            lock (this)
            {
                if (!loggedSoftUsers.TryRemove(softUser.Id, out _))
                {
                    throw new MyException($"User {softUser.Id} is not logged in.");
                }
            }
        }

        // -------------------- TRIP

        public IEnumerable<Trip> GetAllTrips()
        {
            lock (this)
            {
                return tripRepo.FindAll();
            }
        }

        public IEnumerable<Trip> SearchTripsByObjectiveAndTime(string objective, DateTime date, int startHour, int endHour)
        {
            lock (this)
            {
                return tripRepo.FindTripsByObjectiveDateAndTimeRange(objective, date, startHour, endHour);
            }
        }

        public Trip GetTripById(long id)
        {
            lock (this)
            {
                return tripRepo.FindOne(id);
            }
        }

        public void UpdateAvailableSeats(Trip trip, int newAvailableSeats)
        {
            lock (this)
            {
                trip.availableSeats = newAvailableSeats;
                tripRepo.Update(trip);
            }
        }

        // -------------------- RESERVATION

        public void MakeReservation(string clientName, string clientPhone, int ticketCount, Trip trip)
        {
            lock (this)
            {
                if (trip.availableSeats >= ticketCount)
                {
                    var reservation = new Reservation(clientName, clientPhone, ticketCount, trip);
                    reservationRepo.Save(reservation);
                    UpdateAvailableSeats(trip, trip.availableSeats - ticketCount);

                    foreach (var observer in loggedSoftUsers.Values)
                    {
                        observer.ReservationMade(reservation);
                    }
                }
                else
                {
                    throw new MyException("Not enough available seats.");
                }
            }
        }
    }