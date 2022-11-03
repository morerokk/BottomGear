using BottomGear.PiShock.Devices;
using BottomGear.PiShock.PiShockApi;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomGear.Tests.PiShock
{
    public static class PiShockClientMock
    {
        public static Mock<IPiShockClient> Get
        {
            get
            {
                var clientMock = new Mock<IPiShockClient>();

                clientMock.Setup(c => c.Shock(It.IsAny<PiShockDevice>())).Verifiable();

                return clientMock;
            }
        }
    }
}
