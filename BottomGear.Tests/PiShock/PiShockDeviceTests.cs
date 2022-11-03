using BottomGear.PiShock.Devices;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BottomGear.Tests.PiShock
{
    public class PiShockDeviceTests
    {
        [Fact]
        public void TestNotifyShocked_ShouldHaveCooldown()
        {
            var device = new PiShockDevice()
            {
                Duration = 5
            };

            Assert.True(device.CanShock());

            device.NotifyShocked();

            Assert.False(device.CanShock());
        }
    }
}
