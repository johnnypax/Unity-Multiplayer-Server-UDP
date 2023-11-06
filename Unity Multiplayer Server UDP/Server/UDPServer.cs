using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Unity_Multiplayer_Server_UDP.Server
{
    public class UDPServer
    {
        private UdpClient? listener { get; set; }
        private IPEndPoint? groupEP { get; set; }

        private static UDPServer? Instance;
        public static UDPServer getInstance()
        {
            if(Instance == null)
                Instance = new UDPServer();

            return Instance;
        }
        private UDPServer()
        {
            //TODO: Get ip address and port from appsettings.json
            listener = new UdpClient(11000);
            groupEP = new IPEndPoint(IPAddress.Any, 11000);
        }

        public async Task StartServer()
        {
            try
            {
                while (true)
                {
                    if (listener == null)
                        throw new NullReferenceException();

                    UdpReceiveResult result = await listener.ReceiveAsync();
                    byte[] bytes = result.Buffer;
                    string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    Console.WriteLine($"Received coordinates: {message}");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
