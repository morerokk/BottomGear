using BottomGear.PiShock.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomGear.PiShock.Devices
{
    public class PiShockDevice
    {
        public string ShareCode { get; set; }
        public int Strength { get; set; }
        public int Duration { get; set; }
        public string ParameterName { get; set; }
        public ShockType ShockType { get; set; }
        public string LogName { get; set; }
        public string DeviceId { get; set; }

        public string StrengthParameterName { get; set; }
        public string DurationParameterName { get; set; }

        // Unix time when this device was last shocked
        private long LastShockTime;

        public bool CanShock()
        {
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return (now - LastShockTime) > (long)(Duration * 1000);
        }

        public void NotifyShocked()
        {
            LastShockTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}
