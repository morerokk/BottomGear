using System;

namespace BottomGear.PiShock.PiShockApi
{
    public class ShockRequest
    {
        public string Username { get; set; }
        public string Apikey { get; set; }
        public string Name { get; set; }
        public string Code { get; set; } 
        public string Intensity { get; set; }
        public string Duration { get; set; }
        public string Op { get; set; }
    }
}
