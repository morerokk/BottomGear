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
        private PiShockConfig Config;

        private ConcurrentDictionary<string, VRCAnimatorParam> AnimatorParameters = new ConcurrentDictionary<string, VRCAnimatorParam>();

        private VRCAnimatorParameterInterpreter Interpreter;

        private PiShockClient PiShockClient;

        private Thread UpdateLoopThread;

        private Random random = new Random();

        public bool Running { get; private set; }

        public ShockManager()
        {
            this.Config = PiShockConfigProvider.Config;
            this.Interpreter = new VRCAnimatorParameterInterpreter();
            this.PiShockClient = new PiShockClient();
        }

        public void OnAnimatorParameterChanged(object sender, OscMessage message)
        {
            if (!message.Address.StartsWith("/avatar/parameters/"))
            {
                return;
            }

            var param = Interpreter.InterpretOscMessage(message);
            AnimatorParameters[param.Name] = param;
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
                    foreach(var device in Config.Devices)
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
            // Check for randomized or replacement parameters first
            if(!string.IsNullOrWhiteSpace(device.RandomizeParameterName))
            {
                if (AnimatorParameters.TryGetValue(device.RandomizeParameterName, out var param) && param.BoolValue)
                {
                    // Randomize (lol)
                    device.Strength = random.Next(0, 101);
                    device.Duration = random.Next(0, 16);
                }
            }
            else if (!string.IsNullOrWhiteSpace(device.StrengthParameterName))
            {
                // Parameter is between 0.0f and 1.0f, remap that to 0-100 int
                if(AnimatorParameters.TryGetValue(device.StrengthParameterName, out var param))
                {
                    device.Strength = (int)Math.Round(param.FloatValue * 100, 0);
                }
            }
            else if (!string.IsNullOrWhiteSpace(device.DurationParameterName))
            {
                // Parameter is between 0.0f and 1.0f, remap that to 0-15 int
                if (AnimatorParameters.TryGetValue(device.DurationParameterName, out var param))
                {
                    device.Duration = (int)Math.Round(Remap(param.FloatValue, 0f, 1f, 0f, 15f), 0);
                }
            }

            device.NotifyShocked();
            PiShockClient.Shock(device);
        }

        private static float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }

    }
}
