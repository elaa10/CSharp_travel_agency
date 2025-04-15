using System.Configuration;
using System.Net.Sockets;
using System.Reflection;
using Agentie_turism_transport_csharp.networking;
using Agentie_turism_transport_csharp.persistence;
using log4net;
using log4net.Config;
using networking;
using persistence;
using services;


namespace server
{
    public class StartServer
    {
        private static readonly int DEFAULT_PORT = 55556;
        private static readonly string DEFAULT_IP = "127.0.0.1";
        private static readonly ILog log = LogManager.GetLogger(typeof(StartServer));

        static void Main(string[] args)
        {
            //configurare jurnalizare folosind log4net
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
			
            log.Info("Starting chat server");
            log.Info("Reading properties from app.config ...");
           int port = DEFAULT_PORT;
           String ip = DEFAULT_IP;
           String portS= ConfigurationManager.AppSettings["port"];
           if (portS == null)
           {
               log.Debug("Port property not set. Using default value "+DEFAULT_PORT);
           }
           else
           {
               bool result = Int32.TryParse(portS, out port);
               if (!result)
               {
                   log.Debug("Port property not a number. Using default value "+DEFAULT_PORT);
                   port = DEFAULT_PORT;
                   log.Debug("Portul "+port);
               }
           }
           String ipS=ConfigurationManager.AppSettings["ip"];
           
           if (ipS == null)
           {
               log.Info("Port property not set. Using default value "+DEFAULT_IP);
           }
           log.InfoFormat("Configuration Settings for database {0}",GetConnectionStringByName("chatDB"));
           IDictionary<String, string> props = new SortedList<String, String>();
           props.Add("ConnectionString", GetConnectionStringByName("chatDB"));
           TripDBRepository tripRepo = new TripDBRepository(props);
           ReservationDBRepository reservationRepo = new ReservationDBRepository(props, tripRepo);
           SoftUserDBRepository userRepo = new SoftUserDBRepository(props);
           IServices serviceImpl = new ServicesImpl(userRepo, tripRepo, reservationRepo);

         
           log.DebugFormat("Starting server on IP {0} and port {1}", ip, port);
			//SerialChatServer server = new SerialChatServer(ip,port, serviceImpl);
            JsonServer server = new JsonServer(ip,port, serviceImpl);
            server.Start();
            log.Debug("Server started ...");
            //Console.WriteLine("Press <enter> to exit...");
            Console.ReadLine();
            
        }

        static string GetConnectionStringByName(string name)
        {
            string returnValue = null;
            var settings = ConfigurationManager.ConnectionStrings[name];
            if (settings != null)
            {
                returnValue = settings.ConnectionString;
                log.Debug($"Found connection string for {name}");
            }
            else
            {
                log.Warn($"Connection string {name} not found in configuration");
            }
            return returnValue;
        }
    }

    public class JsonServer : ConcurrentServer
    {
        private IServices server;
        private ClientJsonWorker worker;
        private static readonly ILog log = LogManager.GetLogger(typeof(JsonServer));

        public JsonServer(string host, int port, IServices server) : base(host, port)
        {
            this.server = server;
            log.Debug("Created JsonRpcServer instance");
        }

        protected override Thread CreateWorker(TcpClient client)
        {
            log.Debug("Creating new worker thread for client");
            worker = new ClientJsonWorker(server, client);
            return new Thread(worker.Run);
        }
    }
}