using BottomGear.OSC.Config;
using BottomGear.OSC.Listeners;
using BottomGear.PiShock.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BottomGear
{
    class Program
    {
        static void Main(string[] args)
        {
            // Build config
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false);

            IConfiguration config = builder.Build();

            var oscConfig = config.GetRequiredSection("OscConfig").Get<OscConfig>();
            var piShockConfig = config.GetRequiredSection("PiShockConfig").Get<PiShockConfig>();
            PiShockConfigProvider.Initialize(piShockConfig);
            var consoleCommandHandler = new ConsoleCommandHandler();

            Console.WriteLine("Starting OSC Listener...");
            using (var listener = new OSCListener(IPAddress.Parse(oscConfig.Address), oscConfig.Port))
            {
                using (var shockManager = new ShockManager())
                {
                    listener.MessageReceived += shockManager.OnAnimatorParameterChanged;

                    var listenerThread = new Thread(() => {
                        listener.Start();
                    });

                    listenerThread.Start();

                    shockManager.StartUpdateLoop();

                    Console.WriteLine("Listener started! Waiting for messages...");
                    Console.WriteLine("Type \"quit\" to quit.");

                    // Allow user to input a quit command
                    bool quit = false;
                    while (!quit)
                    {
                        string input = Console.ReadLine();
                        if (input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                        {
                            quit = true;
                        }
                        else
                        {
                            consoleCommandHandler.HandleCommand(input);
                        }
                    }

                    listenerThread.Join();
                }
            }
            
            Console.WriteLine("OSC Listener stopped.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void DebugMessageReceived(object sender, Rug.Osc.Core.OscMessage e)
        {
            Console.WriteLine("Received message:");
            Console.WriteLine(e.ToString());
            Console.WriteLine("Address:");
            Console.WriteLine(e.Address);
            Console.WriteLine("Content:");
            Console.WriteLine(e[0]);
            Console.WriteLine("Datatype:");
            Console.WriteLine(e[0].GetType().ToString());
        }
    }
}
