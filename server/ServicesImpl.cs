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
        private readonly IDictionary<long, IObserver> loggedSoftUsers;

        private static readonly ILog logger = LogManager.GetLogger(typeof(ServicesImpl));

        public ServicesImpl(ISoftUserRepository softUserRepository, ITripRepository tripRepository, IReservationRepository reservationRepository)
        {
            softUserRepo = softUserRepository;
            tripRepo = tripRepository;
            reservationRepo = reservationRepository;
            loggedSoftUsers = new Dictionary<long, IObserver>();
        }

        // -------------------- SOFT USER

        public SoftUser Login(string username, string password, IObserver softUserObserver)
        {
            var loginUser = softUserRepo.FindByUsernameAndPassword(username, password);

            if (loginUser != null)
            {
                if (loggedSoftUsers.ContainsKey(loginUser.Id))
                {
                    throw new MyException("User already logged in.");
                }
                loggedSoftUsers[loginUser.Id]= softUserObserver;
                logger.Info("User logged in: " + loginUser.username);
            }
            else
            {
                throw new MyException("Authentication failed.");
            }
            return loginUser;
        }

        public void Logout(SoftUser softUser, IObserver softUserObserver)
        {
            IObserver loginUser = loggedSoftUsers[softUser.Id];
            if(loginUser == null)
                throw new MyException("User "+softUser.Id+" is not logged in.");
            loggedSoftUsers.Remove(softUser.Id);
        }

        // -------------------- TRIP

        public IEnumerable<Trip> GetAllTrips()
        {
            return tripRepo.FindAll();
           
        }

        public IEnumerable<Trip> SearchTripsByObjectiveAndTime(string objective, DateTime date, int startHour, int endHour)
        {
            return tripRepo.FindTripsByObjectiveDateAndTimeRange(objective, date, startHour, endHour);
        }

        public Trip GetTripById(long id)
        {
            return tripRepo.FindOne(id);
        }

        private void UpdateAvailableSeats(Trip trip, int newAvailableSeats)
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
            if (trip.availableSeats >= ticketCount)
            {
                var reservation = new Reservation(clientName, clientPhone, ticketCount, trip);
                reservationRepo.Save(reservation);
                UpdateAvailableSeats(trip, trip.availableSeats - ticketCount);

                foreach (var observer in loggedSoftUsers.Values)
                {
                    Task.Run(() => observer.ReservationMade(reservation));
                }
            }
            else
            {
                throw new MyException("Not enough available seats.");
            }
        }
    }