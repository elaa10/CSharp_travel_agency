using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using Agentie_turism_transport_csharp;
using log4net;
using log4net.Config;
using networking;
using services;

namespace client
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); // activează stilul modern de UI
            var props = LoadProperties(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user.properties"));


            string serverIp = props.ContainsKey("server.host") ? props["server.host"] : "localhost";
            int serverPort = props.ContainsKey("server.port") && int.TryParse(props["server.port"], out var port)
                ? port
                : 55556;

            Console.WriteLine($"Using server IP: {serverIp}");
            Console.WriteLine($"Using server port: {serverPort}");

            IServices service = new ServerJsonProxy(serverIp, serverPort);

            var loginForm = new LoginForm(service); 
            Application.Run(loginForm);
        }

        static Dictionary<string, string> LoadProperties(string path)
        {
            var props = new Dictionary<string, string>();
            if (!File.Exists(path)) return props;

            foreach (var line in File.ReadAllLines(path))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#")) continue;

                var split = trimmed.Split('=', 2);
                if (split.Length == 2)
                    props[split[0].Trim()] = split[1].Trim();
            }

            return props;
        }
    }
}