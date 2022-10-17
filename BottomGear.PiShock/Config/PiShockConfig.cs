using BottomGear.PiShock.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomGear.PiShock.Config
{
    public class PiShockConfig
    {
        public string Username { get; set; }
        public string ApiKey { get; set; }
        public string ApiEndpoint { get; set; }
        public PiShockDevice[] Devices { get; set; }

        // Global overrides that can be set by console command on the fly
        public int? ShockStrengthOverride { get; set; }
        public int? ShockDurationOverride { get; set; }
        public bool? Debug { get; set; }
    }
}
