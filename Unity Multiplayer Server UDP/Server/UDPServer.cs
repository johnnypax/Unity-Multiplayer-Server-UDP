using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Unity_Multiplayer_Server_UDP.Config;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

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
            if (ConfigManager.getInstance().SetupComplete)
            {
                listener = new UdpClient(ConfigManager.getInstance().Port);
                groupEP = new IPEndPoint(IPAddress.Any, ConfigManager.getInstance().Port);
            }
            else
            {
                Console.WriteLine("[ERROR] Error in configuration file");
            }
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

                    ParseCoordinates(message);

                    //Console.WriteLine($"Received coordinates: {message}");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /*
         * message type: 
         * string message = $"{uniqueID}:{position.x};{position.y};{position.z};{rotation.x};{rotation.y};{rotation.z}";
         */
        private void ParseCoordinates(string message)
        {
            string[] parts = message.Split(':');

            if (parts.Length == 2)
            {
                string uniqueID = parts[0];

                string[] vectors = parts[1].Split(';');

                if (vectors.Length == 6)
                {
                    float posX = 0f, posY = 0f, posZ = 0f, rotX = 0f, rotY = 0f, rotZ = 0f;
                    bool parseSuccess = float.TryParse(vectors[0], out posX) &&
                                        float.TryParse(vectors[1], out posY) &&
                                        float.TryParse(vectors[2], out posZ) &&
                                        float.TryParse(vectors[3], out rotX) &&
                                        float.TryParse(vectors[4], out rotY) &&
                                        float.TryParse(vectors[5], out rotZ);

                    if (parseSuccess)
                    {
                        Console.Clear();                    //Clear console for better reading, comment this if you want all complete log

                        //TODO: Store coordinates and reuse for future
                        Console.WriteLine($"{uniqueID}");
                        Console.WriteLine($"P:{posX}|{posY}|{posZ}");
                        Console.WriteLine($"R:{rotX}|{rotY}|{rotZ}");
                    }
                    else
                    {
                        Console.WriteLine("Error parsing position and rotation coordinates.");
                    }
                }
                else
                {
                    Console.WriteLine("Structure of Position and Rotation not correct.");
                }
            }
            else
            {
                Console.WriteLine("Message structure not compatible.");
            }
        }
    }
}
