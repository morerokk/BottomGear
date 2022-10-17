using System;
using System.Collections.Generic;
using System.Text;

namespace BottomGear.OSC.Config
{
    public static class OscConfigProvider
    {
        public static OscConfig Config { get; private set; }

        public static void Initialize(OscConfig config)
        {
            Config = config;
        }
    }
}
