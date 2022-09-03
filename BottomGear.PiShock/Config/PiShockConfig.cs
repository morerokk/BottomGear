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
        public PiShockDevice[] Devices { get; set; }
    }
}
