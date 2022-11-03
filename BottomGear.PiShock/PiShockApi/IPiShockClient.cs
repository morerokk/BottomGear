using BottomGear.PiShock.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomGear.PiShock.PiShockApi
{
    public interface IPiShockClient : IDisposable
    {
        void Shock(PiShockDevice device);
    }
}
