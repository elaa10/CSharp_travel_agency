using System.Net.Sockets;
using System.Text;
using System.Text.Json;

using log4net;
using model;
using networking.dto;
using services;

namespace networking;

public class ClientJsonWorker : IObserver
    {
        private IServices server;
        private TcpClient connection;
        private NetworkStream stream;
        private volatile bool connected;
        private static readonly ILog log = LogManager.GetLogger(typeof(ClientJsonWorker));

        public ClientJsonWorker(IServices server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
                connected = true;
                log.Info("Worker created");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public void Run()
        {
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            while (connected)
            {
                try
                {
                    string requestJson = reader.ReadLine();
                    if (string.IsNullOrEmpty(requestJson)) continue;
                    
                    log.DebugFormat("Received json request {0}", requestJson);
                    Request request = JsonSerializer.Deserialize<Request>(requestJson);
                    log.DebugFormat("Deserialized Request {0}", request);
                    
                    Response response = HandleRequest(request);
                    if (response != null)
                    {
                        SendResponse(response);
                    }
                }
                catch (Exception e)
                {
                    log.ErrorFormat("Error in worker (reading): {0}", e.Message);
                    if (e.InnerException != null)
                        log.ErrorFormat("Inner error: {0}", e.InnerException.Message);
                    log.Error(e.StackTrace);
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    log.ErrorFormat("Error in worker (sleeping): {0}", e.Message);
                }
            }

            try
            {
                stream.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error in worker (closing connection): {0}", e.Message);
            }
        }

        private static readonly Response okResponse = JsonProtocolUtils.CreateOkResponse();

        private Response HandleRequest(Request request)
        {
            Response response = null;
            
            if (request.Type == RequestType.LOGIN)
            {
                log.Debug("Login request ...");
                string[] credentials = JsonSerializer.Deserialize<string[]>(request.Data.ToString());
                string username = credentials[0];
                string password = credentials[1];
                try
                {
                    lock (server)
                    {
                        SoftUser foundSoftUser = server.Login(username, password, this);
                        log.Info("User logged in");
                        return JsonProtocolUtils.CreateOkResponse(foundSoftUser);
                    }
                }
                catch (MyException e)
                {
                    connected = false;
                    log.ErrorFormat("Error in worker (solving method handleLOGIN): {0}", e.Message);
                    return JsonProtocolUtils.CreateErrorResponse(e.Message);
                }
            }

            if (request.Type == RequestType.LOGOUT)
            {
                log.Debug("Logout request ...");
                SoftUser softUser = DTOUtils.GetFromDTO(request.User);
                try
                {
                    lock (server)
                    {
                        server.Logout(softUser, this);
                    }
                    connected = false;
                    log.Info("User logged out");
                    return okResponse;
                }
                catch (MyException e)
                {
                    log.ErrorFormat("Error in worker (solving method handleLOGOUT): {0}", e.Message);
                    return JsonProtocolUtils.CreateErrorResponse(e.Message);
                }
            }

            if (request.Type == RequestType.MAKE_RESERVATION)
            {
                log.Debug("Make reservation request ...");
                Reservation reservation = DTOUtils.GetFromDTO(request.Reservation);
                try
                {
                    lock (server)
                    {
                        server.MakeReservation(reservation.clientName, reservation.clientPhone, reservation.ticketCount, reservation.trip);
                    }
                    log.InfoFormat("Reservation successful for {0}, {1} tickets", reservation.clientName, reservation.ticketCount);
                    return JsonProtocolUtils.CreateReservationMadeResponse(reservation);
                }
                catch (MyException e)
                {
                    log.ErrorFormat("Error in worker (solving method handleMAKE_RESERVATION): {0}", e.Message);
                    return JsonProtocolUtils.CreateErrorResponse(e.Message);
                }
            }

            if (request.Type == RequestType.GET_ALL_TRIPS)
            {
                log.Debug("Get all trips request ...");
                try
                {
                    List<Trip> trips;
                    lock (server)
                    {
                        trips = server.GetAllTrips().ToList(); ;
                    }
                    log.InfoFormat("Trips found: {0}", trips);
                    return JsonProtocolUtils.CreateGetAllTripsResponse(trips);
                }
                catch (MyException e)
                {
                    log.ErrorFormat("Error in worker (solving method handleGET_ALL_TRIPS): {0}", e.Message);
                    return JsonProtocolUtils.CreateErrorResponse(e.Message);
                }
            }

            if (request.Type == RequestType.GET_ALL_TRIPS_BY_DATE)
            {
                log.Debug("Get trips by date request ...");

                if (request.Data is JsonElement dataElement && dataElement.ValueKind == JsonValueKind.Array)
                {
                    var data = dataElement.EnumerateArray().Select(x => x.GetString()).ToList();

                    if (data == null || data.Count != 4)
                    {
                        log.Error("Data array is not valid!");
                        return JsonProtocolUtils.CreateErrorResponse("Invalid data format!");
                    }

                    SearchTripDTO searchTripDTO = new SearchTripDTO()
                    {
                        Objective = data[0],
                        Date = DateTime.ParseExact(data[1], "dd/MM/yyyy HH:mm:ss", null),
                        StartHour = int.Parse(data[2]),
                        EndHour = int.Parse(data[3])
                    };

                    try
                    {
                        List<Trip> trips;
                        lock (server)
                        {
                            trips = server.SearchTripsByObjectiveAndTime(
                                searchTripDTO.Objective,
                                searchTripDTO.Date,
                                searchTripDTO.StartHour,
                                searchTripDTO.EndHour
                            ).ToList();
                        }
                        log.InfoFormat("Trips found: {0}", trips);
                        return JsonProtocolUtils.CreateGetAllTripsByDateResponse(trips);
                    }
                    catch (MyException e)
                    {
                        log.ErrorFormat("Error in worker (solving method handleGET_ALL_TRIPS_BY_DATE): {0}", e.Message);
                        return JsonProtocolUtils.CreateErrorResponse(e.Message);
                    }
                }
                else
                {
                    log.Error("Data is not a JSON array!");
                    return JsonProtocolUtils.CreateErrorResponse("Invalid data format!");
                }
            }


            if (request.Type == RequestType.FIND_TRIP)
            {
                log.Debug("Find trip request ...");
                long tripId = JsonSerializer.Deserialize<long>(request.Data.ToString());
                try
                {
                    Trip trip;
                    lock (server)
                    {
                        trip = server.GetTripById(tripId);
                    }
                    log.InfoFormat("Trip found: {0}", trip);
                    return JsonProtocolUtils.CreateFindTripResponse(trip);
                }
                catch (MyException e)
                {
                    log.ErrorFormat("Error in worker (solving method handleFIND_TRIP): {0}", e.Message);
                    return JsonProtocolUtils.CreateErrorResponse(e.Message);
                }
            }

            return response;
        }

        private void SendResponse(Response response)
        {
            string jsonString = JsonSerializer.Serialize(response);
            log.DebugFormat("sending response {0}", jsonString);
            lock (stream)
            {
                byte[] data = Encoding.UTF8.GetBytes(jsonString + "\n");
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
        }

        public void ReservationMade(Reservation reservation)
        {
            log.DebugFormat("Reservation made for {0}, {1} tickets", reservation.clientName, reservation.ticketCount);
            Response updateResponse = JsonProtocolUtils.CreateReservationMadeResponse(reservation);
            try
            {
                if (connection != null && connection.Connected)
                {
                    log.DebugFormat("Sending update response to other clients: {0}", updateResponse);
                    SendResponse(updateResponse);
                }
                else
                {
                    log.Warn("Connection is not active, cannot send update");
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error sending update response: {0}", e.Message);
            }
        }
    }