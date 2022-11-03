using BottomGear.OSC.Config;
using BottomGear.OSC.Interpreters;
using BottomGear.OSC.Messages;
using BottomGear.PiShock.Config;
using BottomGear.PiShock.Devices;
using BottomGear.PiShock.PiShockApi;
using Rug.Osc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BottomGear
{
    public class ShockManager : IDisposable
    {
        private ConcurrentDictionary<string, VRCAnimatorParam> AnimatorParameters = new ConcurrentDictionary<string, VRCAnimatorParam>();

        private readonly VRCAnimatorParameterInterpreter Interpreter;

        private readonly IPiShockClient PiShockClient;

        private Thread UpdateLoopThread;

        public bool Running { get; private set; }

        public ShockManager() : this(new VRCAnimatorParameterInterpreter(), new PiShockClient())
        {

        }

        public ShockManager(VRCAnimatorParameterInterpreter interpreter, IPiShockClient piShockClient)
        {
            this.Interpreter = interpreter;
            this.PiShockClient = piShockClient;
        }

        public void OnAnimatorParameterChanged(object sender, OscMessage message)
        {
            if (OscConfigProvider.Config.Debug)
            {
                Console.WriteLine("Received OSC message:");
                Console.WriteLine(message.Address);
                Console.WriteLine(message[0]);
            }

            if (message.Address.StartsWith("/avatar/parameters/"))
            {
                var param = Interpreter.InterpretOscMessage(message);
                AnimatorParameters[param.Name] = param;
            }
            else if(message.Address.StartsWith("/avatar/change"))
            {
                // On avatar change, reset the dictionary
                AnimatorParameters.Clear();
            }
        }

        public void StartUpdateLoop()
        {
            if(UpdateLoopThread != null || Running)
            {
                throw new InvalidOperationException("Listener thread on this manager has already been started!");
            }

            Running = true;

            UpdateLoopThread = new Thread(() => {
                while(Running)
                {
                    foreach(var device in PiShockConfigProvider.Config.Devices)
                    {
                        if(AnimatorParameters.TryGetValue(device.ParameterName, out VRCAnimatorParam param) && param.BoolValue && device.CanShock())
                        {
                            Shock(device);
                        }
                    }
                    Thread.Sleep(25);
                }
            });

            UpdateLoopThread.Start();
        }

        public void Dispose()
        {
            Running = false;
            PiShockClient.Dispose();
        }

        private void Shock(PiShockDevice device)
        {
            // Check for replacement parameters
            if (!string.IsNullOrWhiteSpace(device.StrengthParameterName))
            {
                // Parameter is between 0.0f and 1.0f, remap that to 0-100 int
                if (AnimatorParameters.TryGetValue(device.StrengthParameterName, out var param))
                {
                    device.Strength = (int)Math.Round(param.FloatValue * 100, 0);
                }
            }
            if (!string.IsNullOrWhiteSpace(device.DurationParameterName))
            {
                // Parameter is between 0.0f and 1.0f, remap that to 0-10 int
                if (AnimatorParameters.TryGetValue(device.DurationParameterName, out var param))
                {
                    device.Duration = (int)Math.Round(param.FloatValue * 10, 0);
                }
            }

            device.NotifyShocked();
            PiShockClient.Shock(device);
        }
    }
}
