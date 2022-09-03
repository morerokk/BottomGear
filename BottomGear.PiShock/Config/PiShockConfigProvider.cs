using System;
using System.Collections.Generic;
using System.Text;

namespace BottomGear.PiShock.Config
{
    public static class PiShockConfigProvider
    {
        public static PiShockConfig Config { get; private set; }

        public static void Initialize(PiShockConfig config)
        {
            if(config.ApiKey == "(provide PiShock API key here)")
            {
                throw new ArgumentException("Invalid API key! Make sure to edit config.json with your own account and device parameters.", nameof(config));
            }

            Config = config;
        }
    }
}
