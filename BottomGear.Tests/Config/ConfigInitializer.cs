using BottomGear.OSC.Config;
using BottomGear.PiShock.Config;
using BottomGear.PiShock.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomGear.Tests.Config
{
    public static class ConfigInitializer
    {
        public static void Initialize()
        {
            var oscConfig = new OscConfig()
            {
                Address = "127.0.0.1",
                Debug = false,
                Port = 9001
            };

            OscConfigProvider.Initialize(oscConfig);


            var piShockConfig = new PiShockConfig()
            {
                ApiEndpoint = "http://localhost",
                ApiKey = "apikey",
                Debug = false,
                Devices = new PiShockDevice[]
                {
                    new PiShockDevice()
                    {
                        DeviceId = "123",
                        Duration = 2,
                        Strength = 20,
                        DurationParameterName = "ShockDuration",
                        StrengthParameterName = "ShockStrength",
                        LogName = "TEST",
                        ParameterName = "ShockMe",
                        ShareCode = "1235467",
                        ShockType = ShockType.Shock
                    }
                },
                Username = "Username"
            };

            PiShockConfigProvider.Initialize(piShockConfig);
        }
    }
}
