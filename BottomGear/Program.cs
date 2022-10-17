using BottomGear.OSC.Config;
using BottomGear.OSC.Listeners;
using BottomGear.PiShock.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace BottomGear
{
    class Program
    {
        // Unfortunate bits of code that we need to handle CTRL+C cleanly and escape from Console.ReadLine().
        const int STD_INPUT_HANDLE = -10;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);

        static void Main(string[] args)
        {
            // Build config
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false);

            IConfiguration config = builder.Build();

            // Initialize config singletons
            var oscConfig = config.GetRequiredSection("OscConfig").Get<OscConfig>();
            var piShockConfig = config.GetRequiredSection("PiShockConfig").Get<PiShockConfig>();
            OscConfigProvider.Initialize(oscConfig);
            PiShockConfigProvider.Initialize(piShockConfig);
            

            Console.WriteLine("Starting OSC Listener...");

            Thread listenerThread = null;

            using (var listener = new OSCListener(IPAddress.Parse(oscConfig.Address), oscConfig.Port))
            {
                using (var shockManager = new ShockManager())
                {
                    listener.MessageReceived += shockManager.OnAnimatorParameterChanged;

                    listenerThread = new Thread(() => {
                        listener.Start();
                    });

                    listenerThread.Start();

                    shockManager.StartUpdateLoop();

                    Console.WriteLine("Listener started! Waiting for messages...");
                    Console.WriteLine("Type \"quit\" to quit.");

                    // Allow user to input a quit command or press CTRL+C to exit
                    bool quit = false;

                    Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs args) => {
                        quit = true;
                        // Tell the OS that we are handling the CTRL+C event ourselves and not to kill the process
                        args.Cancel = true;
                        // Send an enter press to the console window to close it
                        var handle = GetStdHandle(STD_INPUT_HANDLE);
                        CancelIoEx(handle, IntPtr.Zero);
                    };

                    while (!quit)
                    {
                        try
                        {
                            string input = Console.ReadLine();
                            if ("quit".Equals(input, StringComparison.OrdinalIgnoreCase))
                            {
                                quit = true;
                            }
                        }
                        catch (InvalidOperationException) { }
                        catch (OperationCanceledException) { }
                    }
                }
            }

            // Wait for the listener thread to end gracefully
            if (listenerThread != null)
            {
                listenerThread.Join();
            }

            Console.WriteLine("OSC Listener stopped.");
        }
    }
}
