using BottomGear.OSC.Interpreters;
using BottomGear.OSC.Messages;
using BottomGear.PiShock.Config;
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
                            device.NotifyShocked();
                            PiShockClient.Shock(device);
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
    }
}
