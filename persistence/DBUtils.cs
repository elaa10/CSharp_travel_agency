using System.Data;
using System.Data.SQLite;
using System.Reflection;
using log4net;


namespace persistence;

public static class DBUtils
{
    private static IDbConnection instance = null;

    public static IDbConnection GetConnection(IDictionary<string, string> props)
    {
        if (instance == null || instance.State == System.Data.ConnectionState.Closed)
        {
            instance = GetNewConnection(props);
            instance.Open();
        }
        return instance;
    }


    private static IDbConnection GetNewConnection(IDictionary<string, string> props)
    {
        return ConnectionFactory.GetInstance().CreateConnection(props);
    }
}

public abstract class ConnectionFactory
{
    protected ConnectionFactory() { }

    private static ConnectionFactory instance;

    public static ConnectionFactory GetInstance()
    {
        if (instance == null)
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            Type[] types = assem.GetTypes();
            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(ConnectionFactory)))
                    instance = (ConnectionFactory)Activator.CreateInstance(type);
            }
        }
        return instance;
    }
    public abstract IDbConnection CreateConnection(IDictionary<string, string> props);
}

public class SQLiteConnectionFactory : ConnectionFactory
{
    private static readonly ILog log = LogManager.GetLogger(typeof(SQLiteConnectionFactory));

    public override IDbConnection CreateConnection(IDictionary<string, string> props)
    {
        String connectionString = props["ConnectionString"];
        log.DebugFormat("creating ... sqlite connection for {0}", connectionString);
        Console.WriteLine(@"SQLite --- opens a connection to {0}", connectionString);
        return new SQLiteConnection(connectionString);
    }
}
