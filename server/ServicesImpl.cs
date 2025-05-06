// using log4net;
// using model;
// using persistence.interfaces;
// using services;
//
// namespace server;
//
// public class ServicesImpl : IServices
//     {
//         private readonly ISoftUserRepository softUserRepo;
//         private readonly ITripRepository tripRepo;
//         private readonly IReservationRepository reservationRepo;
//         private readonly IDictionary<string, IObserver> loggedSoftUsers;
//
//         private static readonly ILog logger = LogManager.GetLogger(typeof(ServicesImpl));
//
//         public ServicesImpl(ISoftUserRepository softUserRepository, ITripRepository tripRepository, IReservationRepository reservationRepository)
//         {
//             softUserRepo = softUserRepository;
//             tripRepo = tripRepository;
//             reservationRepo = reservationRepository;
//             loggedSoftUsers = new Dictionary<string, IObserver>();
//         }
//
//         // -------------------- SOFT USER
//
//         public void Login(SoftUser softUser, IObserver softUserObserver)
//         {
//             Console.WriteLine("Login in Server");
//             var loginUser = softUserRepo.FindByUsernameAndPassword(softUser.username, softUser.password);
//
//             if (loginUser != null)
//             {
//                 if (loggedSoftUsers.ContainsKey(loginUser.username))
//                 {
//                     throw new MyException("User already logged in.");
//                 }
//                 loggedSoftUsers[loginUser.username]= softUserObserver;
//                 logger.Info("User logged in: " + loginUser.username);
//             }
//             else
//             {
//                 throw new MyException("Authentication failed.");
//             } 
//         }
//
//         public void Logout(SoftUser softUser, IObserver softUserObserver)
//         {
//             IObserver loginUser = loggedSoftUsers[softUser.username];
//             if(loginUser == null)
//                 throw new MyException("User "+softUser.Id+" is not logged in.");
//             loggedSoftUsers.Remove(softUser.username);
//         }
//
//         // -------------------- TRIP
//
//         public IEnumerable<Trip> GetAllTrips()
//         {
//             logger.Info("Getting all trips");
//             return tripRepo.FindAll();
//            
//         }
//
//         public IEnumerable<Trip> SearchTripsByObjectiveAndTime(string objective, DateTime date, int startHour, int endHour)
//         {
//             logger.Info("Searching trips for: " + objective);
//             return tripRepo.FindTripsByObjectiveDateAndTimeRange(objective, date, startHour, endHour);
//         }
//
//         public Trip GetTripById(long id)
//         {
//             logger.Info("Getting trip by id: " + id);
//             Trip trip = tripRepo.FindOne(id);
//             if (trip == null)
//                 throw new MyException("Trip " + id + " is not found.");
//             return trip;
//         }
//
//         private void UpdateAvailableSeats(Trip trip, int newAvailableSeats)
//         {
//             lock (this)
//             {
//                 trip.availableSeats = newAvailableSeats;
//                 tripRepo.Update(trip);
//             }
//         }
//
//         // -------------------- RESERVATION
//
//         public void MakeReservation(string clientName, string clientPhone, int ticketCount, Trip trip)
//         {
//             if (trip.availableSeats >= ticketCount)
//             {
//                 var reservation = new Reservation(clientName, clientPhone, ticketCount, trip);
//                 reservationRepo.Save(reservation);
//                 UpdateAvailableSeats(trip, trip.availableSeats - ticketCount);
//
//                 foreach (var loggedUser in loggedSoftUsers)
//                 {
//                     Console.WriteLine("Notifying client " + loggedUser.Key);
//                     IObserver observer = loggedUser.Value;
//                     try
//                     {
//                         Task.Run(() => observer.ReservationMade(reservation));
//                     }
//                     catch (MyException e)
//                     {
//                         logger.Error("Error notifying client", e);
//                     }
//                     catch (Exception e)
//                     {
//                         logger.Error("Error notifying client", e);
//                     }
//                 }
//             }
//             else
//             {
//                 throw new MyException("Not enough available seats.");
//             }
//         }
//     }