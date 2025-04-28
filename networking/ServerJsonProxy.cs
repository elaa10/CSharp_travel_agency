using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Agentie_turism_transport_csharp.networking;
using log4net;
using model;
using networking.dto;
using services;

namespace networking;

public class ServerJsonProxy : IServices
    {
        private string host;
        private int port;

        private IObserver softUserObserver;
        private NetworkStream stream;
        private TcpClient connection;
        private Queue<Response> responses;
        private volatile bool finished;
        private EventWaitHandle _waitHandle;
        private static readonly ILog log = LogManager.GetLogger(typeof(ServerJsonProxy));

        public ServerJsonProxy(string host, int port)
        {
            log.Info("Creating proxy");
            this.host = host;
            this.port = port;
            responses = new Queue<Response>();
        }

        public void Login(SoftUser softUser, IObserver softUserObserver)
        {
            Console.WriteLine("Login user " + softUser);;
            InitializeConnection();
            SendRequest(JsonProtocolUtils.CreateLoginRequest(softUser));

            Response response = ReadResponse();
            if (response.Type == ResponseType.OK)
            {
                this.softUserObserver = softUserObserver;
                log.Info("Logged in {0}");
            }

            if (response.Type == ResponseType.ERROR)
            {
                log.Error("Error logging in" + response.ErrorMessage);
                string err = response.ErrorMessage;
                CloseConnection();
                throw new MyException(err);
            }
        }

        public void Logout(SoftUser softUser, IObserver client)
        {
            Request req = JsonProtocolUtils.CreateLogoutRequest(softUser);
            SendRequest(req);
            Response response = ReadResponse();
            CloseConnection();
            if (response.Type == ResponseType.ERROR)
            {
                log.Error("Error logging out" + response.ErrorMessage);
                string err = response.ErrorMessage;
                throw new MyException(err);
            }
            log.Info("Logged out");
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            SendRequest(JsonProtocolUtils.CreateGetAllTripsRequest());
            Response response = ReadResponse();

            if (response.Type == ResponseType.ERROR)
            {
                log.Error("Error getting trips" + response.ErrorMessage);
                string err = response.ErrorMessage;
                throw new MyException(err);
            }
            log.Info("Got trips");
            
            Trip[] trips = response.Trips;
            return trips;
        }

        public IEnumerable<Trip> SearchTripsByObjectiveAndTime(string objective, DateTime date, int startHour, int endHour)
        {
            var searchTripDTO = new SearchTripDTO(objective, date, startHour, endHour);
            SendRequest(JsonProtocolUtils.CreateGetTripsByDateRequest(searchTripDTO));

            Response response = ReadResponse();

            if (response.Type == ResponseType.ERROR)
            {
                log.Error("Error getting trips" + response.ErrorMessage);
                string err = response.ErrorMessage;
                throw new MyException(err);
            }

            log.Info("Got trips");
            Trip[] trips = response.Trips;
            return trips;
        }

        public Trip GetTripById(long id)
        {
            SendRequest(JsonProtocolUtils.CreateFindTripRequest(id));
            Response response = ReadResponse();

            if (response.Type == ResponseType.ERROR)
            {
                log.Error("Error finding trip" + response.ErrorMessage);
                string err = response.ErrorMessage;
                throw new MyException(err);
            }
            log.Info("Found trip");
            Trip trip = response.Trip;
            return trip;
        }

        public void MakeReservation(string clientName, string clientPhone, int ticketCount, Trip trip)
        {
            Reservation reservation = new Reservation(clientName, clientPhone, ticketCount, trip);
            SendRequest(JsonProtocolUtils.CreateMakeReservationRequest(reservation));
            
            Response response = ReadResponse();
            if (response.Type == ResponseType.ERROR)
            {
                log.Error("Error making reservation" + response.ErrorMessage);
                string err = response.ErrorMessage;
                throw new MyException(err);
            }
            log.Info("Reservation made");
        }

        private void InitializeConnection()
        {
            try
            {
                log.Info($"Attempting to connect to {host}:{port}");
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                finished = false;
                _waitHandle = new AutoResetEvent(false);
                StartReader();
                log.Info("Connection initialized successfully");
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error initializing connection {0}", e.Message);
                if (e.InnerException != null)
                    log.ErrorFormat("Error initializing connection inner error {0}", e.InnerException.Message);
                Console.WriteLine("Error initializing connection " + e.Message);
            }
        }

        private void CloseConnection()
        {
            finished = true;
            try
            {
                stream.Close();
                connection.Close();
                _waitHandle.Close();
                softUserObserver = null;
                log.Info("Closed connection");
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error closing connection {0}", e.Message);
                if (e.InnerException != null)
                    log.ErrorFormat("Error closing connection inner error {0}", e.InnerException.Message);
            }
        }

        private void SendRequest(Request request)
        {
            try
            {
                if (stream == null)
                {
                    throw new MyException("Stream is null! Probably you are not connected to server.");
                }
                lock (stream)
                {
                    string jsonRequest = JsonSerializer.Serialize(request);
                    log.Debug($"Sending request: {jsonRequest}");
                    byte[] data = Encoding.UTF8.GetBytes(jsonRequest + "\n");
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error sending request {0}", e.Message);
                if (e.InnerException != null)
                    log.ErrorFormat("Error sending request inner error {0}", e.InnerException.Message);
                throw new MyException("Error sending request " + e);
            }
        }
        private Response ReadResponse()
        {
            Response response = null;
            try
            {
                _waitHandle.WaitOne();
                lock (responses)
                {
                    response = responses.Dequeue();
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error reading response {0}", e.Message);
                if (e.InnerException != null)
                    log.ErrorFormat("Error reading response inner error {0}", e.InnerException.Message);    
            }
            return response;
        }

        private void StartReader()
        {
            Thread tw = new Thread(Run);
            tw.Start();
        }

        private void HandleUpdate(Response response)
        {
            log.DebugFormat("handleUpdate called with {0}",response);
            if (response.Type == ResponseType.RESERVATION_MADE)
            {
                try
                {
                    log.Debug($"Attempting to notify trip : {response.Reservation != null}");
                    if (softUserObserver == null)
                    {
                        log.Error("Client is null when trying to notify about ticket");
                        return;
                    }
                    try
                    {
                        softUserObserver.ReservationMade(response.Reservation);
                    }
                    catch (Exception ex) {
                        log.Error($"Failed to notify client: {ex.Message}");
                    }
                }
                catch (Exception e)
                {
                    log.ErrorFormat("Error handling update {0}", e.Message);
                    if (e.InnerException != null)
                        log.ErrorFormat("Error handling update inner error {0}", e.InnerException.Message);
                }
            }
        }

        private bool IsUpdate(Response response)
        {
            return response.Type == ResponseType.RESERVATION_MADE;
        }

        private void Run()
        {
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            while (!finished)
            {
                try
                {
                    string responseJson = reader.ReadLine();
                    if (string.IsNullOrEmpty(responseJson))
                        continue;
                    log.DebugFormat("Received json response {0}", responseJson);

                    Response response = JsonSerializer.Deserialize<Response>(responseJson);
                    log.Info("response received " + response);
                    log.DebugFormat("Deserialized response {0}", JsonSerializer.Serialize(response));


                    if (IsUpdate(response))
                    {
                        HandleUpdate(response);
                    }
                    else
                    {
                        lock (responses)
                        {
                            responses.Enqueue(response);
                        }
                        _waitHandle?.Set();
                    }
                }
                catch (Exception e)
                {
                    log.ErrorFormat("Error reading response {0}", e.Message);
                    if (e.InnerException != null)
                        log.ErrorFormat("Error reading response inner error {0}", e.InnerException.Message);
                }
            }
        }
    }