using System.Configuration;
using System.Reflection;
using log4net;
using log4net.Config;
using networking;
using services;


namespace clientWinFormApp;

internal class StartClient{

    private static int DEFAULT_PORT = 55556;
    private static string DEFAULT_IP = "127.0.0.1";
    private static readonly ILog log = LogManager.GetLogger(typeof(StartClient));
    
    [STAThread]
    static void Main()
    {
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        
        log.Info("Apllication started");
        int port = DEFAULT_PORT;
        string host = DEFAULT_IP;
        string portS = ConfigurationManager.AppSettings["port"];
        if (portS == null)
        {
            log.DebugFormat("Port not specified, using default {0}", DEFAULT_PORT);
        }
        else
        {
            bool result = Int32.TryParse(portS, out port);
            if (!result)
            {
                log.ErrorFormat("Port {0} is not a number, using default {1}", portS, DEFAULT_PORT);
                port = DEFAULT_PORT;
            }
        }
        
        string ipS = ConfigurationManager.AppSettings["ip"];
        if (ipS == null)
        {
            log.DebugFormat("IP not specified, using default {0}", DEFAULT_IP);
        }
        else
        {
            host = ipS;
            log.DebugFormat("Using IP {0}", host);
        }
        
        IServices server = new ServerJsonProxy(host, port);
        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm(server));
    }
}