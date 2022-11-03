using BottomGear.OSC.Interpreters;
using BottomGear.PiShock.Devices;
using BottomGear.Tests.Config;
using BottomGear.Tests.PiShock;
using Moq;
using Rug.Osc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace BottomGear.Tests.BottomGear
{
    public class ShockManagerTests
    {
        [Fact]
        public void TestAnimatorParameterChanged_GivenTrue_ShouldCallShock()
        {
            ConfigInitializer.Initialize();

            var piShockClient = PiShockClientMock.Get;
            var shockManager = new ShockManager(new VRCAnimatorParameterInterpreter(), piShockClient.Object);

            shockManager.StartUpdateLoop();
            shockManager.OnAnimatorParameterChanged(null, new OscMessage("/avatar/parameters/ShockMe", true));
            Thread.Sleep(100);

            piShockClient.Verify(c => c.Shock(It.IsAny<PiShockDevice>()));
        }
    }
}
