using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcContracts;
using Google.Protobuf.WellKnownTypes;
using model;
using persistence.interfaces;
using services;

namespace server
{
    public class ServiceImplGrpc : TourismService.TourismServiceBase
    {
        private readonly ISoftUserRepository softUserRepo;
        private readonly ITripRepository tripRepo;
        private readonly IReservationRepository reservationRepo;
        private readonly Dictionary<string, IObserver> loggedSoftUsers = new();
        private readonly List<IServerStreamWriter<ReservationNotification>> observers = new();
        private readonly object observersLock = new();


        public ServiceImplGrpc(ISoftUserRepository softUserRepo, ITripRepository tripRepo, IReservationRepository reservationRepo)
        {
            this.softUserRepo = softUserRepo;
            this.tripRepo = tripRepo;
            this.reservationRepo = reservationRepo;
        }

        // ------------------ SoftUser ------------------
        public override Task<LoginResponse> Login(GrpcContracts.SoftUser request, ServerCallContext context)
        {
            var user = softUserRepo.FindByUsernameAndPassword(request.Username, request.Password);

            if (user == null)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Authentication failed."));

            lock (loggedSoftUsers)
            {
                if (loggedSoftUsers.ContainsKey(user.username))
                    throw new RpcException(new Status(StatusCode.AlreadyExists, "User already logged in."));

                loggedSoftUsers[user.username] = null; // placeholder since gRPC doesn't support callbacks
            }

            return Task.FromResult(new LoginResponse { Message = "Login successful" });
        }

        public override Task<Empty> Logout(GrpcContracts.SoftUser request, ServerCallContext context)
        {
            lock (loggedSoftUsers)
            {
                if (!loggedSoftUsers.Remove(request.Username))
                    throw new RpcException(new Status(StatusCode.NotFound, "User not logged in."));
            }

            return Task.FromResult(new Empty());
        }

        // ------------------ Trip ------------------
        public override Task<TripList> GetAllTrips(Empty request, ServerCallContext context)
        {
            var trips = tripRepo.FindAll();
            var list = new TripList();
            list.Trips.AddRange(trips.Select(ToGrpcTrip));
            return Task.FromResult(list);
        }

        public override Task<TripList> SearchTripsByObjectiveAndTime(SearchTripRequest request, ServerCallContext context)
        {
            var date = request.Date.ToDateTime();
            var results = tripRepo.FindTripsByObjectiveDateAndTimeRange(request.Objective, date, request.StartHour, request.EndHour);
            var list = new TripList();
            list.Trips.AddRange(results.Select(ToGrpcTrip));
            return Task.FromResult(list);
        }

        public override Task<GrpcContracts.Trip> GetTripById(TripIdRequest request, ServerCallContext context)
        {
            var trip = tripRepo.FindOne(request.Id);
            if (trip == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Trip not found."));

            return Task.FromResult(ToGrpcTrip(trip));
        }

        // ------------------ Reservation ------------------
        public override async Task<Empty> MakeReservation(ReservationRequest request, ServerCallContext context)
        {
            var trip = FromGrpcTrip(request.Trip);

            if (trip.availableSeats < request.TicketCount)
                throw new RpcException(new Status(StatusCode.FailedPrecondition, "Not enough available seats."));

            var reservation = new Reservation(request.ClientName, request.ClientPhone, request.TicketCount, trip);
            reservationRepo.Save(reservation);
            UpdateAvailableSeats(trip, trip.availableSeats - request.TicketCount);

            Console.WriteLine("[MakeReservation] Notifying all clients...");
            
            lock (observersLock)
            {
                foreach (var observer in observers.ToList())
                {
                    try
                    {
                        observer.WriteAsync(new ReservationNotification
                        {
                            Message = "A reservation was made.",
                            TripId = trip.Id
                        });
                    }
                    catch
                    { }
                }
            }

            return new Empty();
        }


        private void UpdateAvailableSeats(model.Trip trip, int newAvailableSeats)
        {
            lock (this)
            {
                trip.availableSeats = newAvailableSeats;
                tripRepo.Update(trip);
            }
        }

        // ------------------ Helpers ------------------
        private GrpcContracts.Trip ToGrpcTrip(model.Trip trip)
        {
            return new GrpcContracts.Trip
            {
                Id = trip.Id,
                TouristObjective = trip.touristAttraction,
                TransportCompany = trip.transportCompany,
                AvailableSeats = trip.availableSeats,
                Price = trip.price,
                DepartureTime = Timestamp.FromDateTime(trip.departureTime.ToUniversalTime())
            };
        }

        private model.Trip FromGrpcTrip(GrpcContracts.Trip grpcTrip)
        {
            return new model.Trip(
                grpcTrip.TouristObjective,
                grpcTrip.TransportCompany,
                grpcTrip.DepartureTime.ToDateTime(),
                grpcTrip.Price,
                grpcTrip.AvailableSeats
            ) { Id = grpcTrip.Id };
        }
        
        public override async Task ReservationNotifications(Empty request, IServerStreamWriter<ReservationNotification> responseStream, ServerCallContext context)
        {
            lock (observersLock)
            {
                observers.Add(responseStream);
            }
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000); 
            }

            lock (observersLock)
            {
                observers.Remove(responseStream);
            }
        }

    }
}