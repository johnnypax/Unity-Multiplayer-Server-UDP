﻿using Unity_Multiplayer_Server_UDP.Config;
using Unity_Multiplayer_Server_UDP.Server;

namespace Unity_Multiplayer_Server_UDP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[INFO] Server Listening...");
            ConfigManager.getInstance();

            Task serverTask = UDPServer.getInstance().StartServer();
            serverTask.GetAwaiter().GetResult();


            Console.WriteLine("[INFO] Server Terminated");

        }
    }
}