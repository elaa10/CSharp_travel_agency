using System.Net;
using System.Net.Sockets;
using log4net;

namespace networking;

public abstract class AbstractServer
{
    private TcpListener server;
    private string host;
    private int port;
        
    private static readonly ILog log = LogManager.GetLogger(typeof(AbstractServer));

    public AbstractServer(string host, int port)
    {
        this.host = host;
        this.port = port;
    }

    public void Start()
    {
        try
        {
            IPAddress address = IPAddress.Parse(host);
            IPEndPoint endPoint = new IPEndPoint(address, port);
            server = new TcpListener(endPoint);
            server.Start();
            log.Info($"Server started on {host}:{port}");
                
            while (true)
            {
                log.Info("Waiting for clients...");
                TcpClient client = server.AcceptTcpClient();
                log.Info("Client connected...");
                ProcessRequest(client);
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in server: ", ex);
        }
    }

    public abstract void ProcessRequest(TcpClient client);
}

public abstract class ConcurrentServer : AbstractServer
{
    private static readonly ILog log = LogManager.GetLogger(typeof(AbstractServer));
    
    protected ConcurrentServer(string host, int port) : base(host, port)
    {}

    public override void ProcessRequest(TcpClient client)
    {
        try
        {
            Thread workerThread = CreateWorker(client);
            workerThread.Start();
            log.Info("Created and started worker thread for client");
        }
        catch (Exception ex)
        {
            log.Error("Error creating worker thread: ", ex);
        }
    }

    protected abstract Thread CreateWorker(TcpClient client);
}