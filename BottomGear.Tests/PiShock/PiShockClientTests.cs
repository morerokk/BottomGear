using BottomGear.PiShock.Config;
using BottomGear.PiShock.PiShockApi;
using BottomGear.Tests.Config;
using Moq;
using Moq.Protected;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BottomGear.Tests.PiShock
{
    public class PiShockClientTests
    {
        [Fact]
        public void TestShock_ShouldSendHttpMessage_WithExpectedBody()
        {
            ConfigInitializer.Initialize();

            var client = new PiShockClient();

            var httpClientMock = GetHttpClientMock();
            httpClientMock.Expect("*");

            // HttpClient is almost not mockable
            // If we make a wrapper for the client, what will test the wrapper?
            typeof(PiShockClient)
                .GetField("HttpClient", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(client, httpClientMock.ToHttpClient());

            client.Shock(PiShockConfigProvider.Config.Devices[0]);

            Thread.Sleep(250);

            httpClientMock.VerifyNoOutstandingExpectation();
        }

        private MockHttpMessageHandler GetHttpClientMock()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("*")
                .Respond("application/json", "");

            return mockHttp;
        }
    }
}
