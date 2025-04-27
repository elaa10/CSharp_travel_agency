using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Agentie_turism_transport_csharp.networking;
using log4net;
using model;
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

        public SoftUser Login(string username, string password, IObserver softUserObserver)
        {
            InitializeConnection();
            string[] credentials = new string[] { username, password };
            SendRequest(JsonProtocolUtils.CreateLoginRequest(credentials));

            Response response = ReadResponse();
            if (response.Type == ResponseType.OK)
            {
                this.softUserObserver = softUserObserver;
                log.Info("Logged in");
                return JsonSerializer.Deserialize<SoftUser>(response.Data.ToString());
            }

            if (response.Type == ResponseType.ERROR)
            {
                log.Error("Error logging in" + response.ErrorMessage);
                string err = response.ErrorMessage;
                CloseConnection();
                throw new MyException(err);
            }
            return null;
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
            else
            {
                log.Info("Got trips");
            }
            return JsonSerializer.Deserialize<List<Trip>>(response.Data.ToString());
        }

        public IEnumerable<Trip> SearchTripsByObjectiveAndTime(string objective, DateTime date, int startHour, int endHour)
        {
            string[] data = new string[] { objective, date.ToString(), startHour.ToString(), endHour.ToString() };
            SendRequest(JsonProtocolUtils.CreateGetTripsByDateRequest(data));

            Response response = ReadResponse();

            if (response.Type == ResponseType.ERROR)
            {
                log.Error("Error getting trips" + response.ErrorMessage);
                string err = response.ErrorMessage;
                throw new MyException(err);
            }
            else
            {
                log.Info("Got trips");
            }
            return JsonSerializer.Deserialize<List<Trip>>(response.Data.ToString());
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
            else
            {
                log.Info("Found trip");
            }
            return JsonSerializer.Deserialize<Trip>(response.Data.ToString());
        }

        public void MakeReservation(string clientName, string clientPhone, int ticketCount, Trip trip)
        {
            object[] data = new object[] { clientName, clientPhone, ticketCount, trip };
            SendRequest(JsonProtocolUtils.CreateMakeReservationRequest(data));

            Response response = ReadResponse();
            if (response.Type == ResponseType.ERROR)
            {
                log.Error("Error making reservation" + response.ErrorMessage);
                string err = response.ErrorMessage;
                throw new MyException(err);
            }
            else
            {
                log.Info("Reservation made");
            }
        }

        private void InitializeConnection()
        {
            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                finished = false;
                _waitHandle = new AutoResetEvent(false);
                StartReader();
                log.Info("Connection initialized");
            }
            catch (Exception e)
            {
                log.Error("Error connecting to server " + e);
                throw new MyException("Error connecting to server " + e);
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
                log.Error("Error closing connection: " + e);
            }
        }

        private void SendRequest(Request request)
        {
            try
            {
                lock (stream)
                {
                    string jsonRequest = JsonSerializer.Serialize(request);
                    log.DebugFormat("Sending request {0}", jsonRequest);
                    byte[] data = Encoding.UTF8.GetBytes(jsonRequest + "\n");
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                }
            }
            catch (Exception e)
            {
                log.Error("Error sending object " + e);
                throw new MyException("Error sending object " + e);
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
                log.Error("Reading response error " + e);
                throw new MyException("Reading response error " + e);
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
                Reservation reservation = JsonSerializer.Deserialize<Reservation>(response.Data.ToString());
                log.Info("Reservation made " + reservation);
                try
                {
                    softUserObserver.ReservationMade(reservation);
                }
                catch (MyException e)
                {
                    log.Error("Error handle update: " + e);
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

                    Response response = JsonSerializer.Deserialize<Response>(responseJson);
                    log.Info("response received " + response);

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
                        _waitHandle.Set();
                    }
                }
                catch (Exception e)
                {
                    if (e is IOException)
                        log.Info("Socket closed: " + e);
                    else
                        log.Error("Reading error: " + e);
                }
            }
        }
    }