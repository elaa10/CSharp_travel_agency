using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Grpc.Core;
using GrpcContracts;
using Google.Protobuf.WellKnownTypes;
using model;
using services;

namespace server
{
    public class ServiceImplGrpc : TourismService.TourismServiceBase
    {
        private readonly IServices _service;

        public ServiceImplGrpc(IServices service)
        {
            _service = service;
        }

        public override Task<LoginResponse> Login(GrpcContracts.SoftUser request, ServerCallContext context)
        {
            var user = new model.SoftUser(request.Username, request.Password) { Id = request.Id };

            try
            {
                _service.Login(user, null); // sau trimite un observer dacă ai implementat
                return Task.FromResult(new LoginResponse { Message = "Login successful" });
            }
            catch (MyException ex)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, ex.Message));
            }
        }

        public override Task<Empty> Logout(GrpcContracts.SoftUser request, ServerCallContext context)
        {
            var user = new model.SoftUser(request.Username, request.Password) { Id = request.Id };

            try
            {
                _service.Logout(user, null);
                return Task.FromResult(new Empty());
            }
            catch (MyException ex)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, ex.Message));
            }
        }

        public override Task<TripList> GetAllTrips(Empty request, ServerCallContext context)
        {
            var trips = _service.GetAllTrips();

            var tripList = new TripList();
            tripList.Trips.AddRange(trips.Select(ToGrpcTrip));

            return Task.FromResult(tripList);
        }

        public override Task<TripList> SearchTripsByObjectiveAndTime(SearchTripRequest request, ServerCallContext context)
        {
            var date = request.Date.ToDateTime();
            var trips = _service.SearchTripsByObjectiveAndTime(request.Objective, date, request.StartHour, request.EndHour);

            var tripList = new TripList();
            tripList.Trips.AddRange(trips.Select(ToGrpcTrip));

            return Task.FromResult(tripList);
        }

        public override Task<GrpcContracts.Trip> GetTripById(TripIdRequest request, ServerCallContext context)
        {
            var trip = _service.GetTripById(request.Id);
            return Task.FromResult(ToGrpcTrip(trip));
        }


        public override Task<Empty> MakeReservation(ReservationRequest request, ServerCallContext context)
        {
            var trip = FromGrpcTrip(request.Trip);
            try
            {
                _service.MakeReservation(request.ClientName, request.ClientPhone, request.TicketCount, trip);
                return Task.FromResult(new Empty());
            }
            catch (MyException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
        }

        // Helper: model.Trip -> GrpcContracts.Trip
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

        // Helper: GrpcContracts.Trip -> model.Trip
        private model.Trip FromGrpcTrip(GrpcContracts.Trip grpcTrip)
        {
            return new model.Trip(
                grpcTrip.TouristObjective,
                grpcTrip.TransportCompany,
                grpcTrip.DepartureTime.ToDateTime(),
                grpcTrip.Price,
                grpcTrip.AvailableSeats
            )
            {
                Id = grpcTrip.Id
            };
        }
    }
}
