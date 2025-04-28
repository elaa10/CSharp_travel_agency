using System.Net.Sockets;
using networking;
using services;

namespace client;

static class StartClient
{
    [STAThread]
    static void Main()
    {
        try
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Verifică dacă serverul este disponibil
            if (!IsServerAvailable("127.0.0.1", 55556))
            {
                MessageBox.Show("Server is not available. Please start the server first.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IServices server = new ServerJsonProxy("127.0.0.1", 55556);
            ProjectClientCtrl ctrl = new ProjectClientCtrl(server);
            LoginForm win = new LoginForm(ctrl);
            Application.Run(win);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error starting application: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private static bool IsServerAvailable(string host, int port)
    {
        try
        {
            using (var client = new TcpClient())
            {
                var result = client.BeginConnect(host, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                if (!success)
                {
                    return false;
                }
                client.EndConnect(result);
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

}