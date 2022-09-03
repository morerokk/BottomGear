using BottomGear.PiShock.Config;
using BottomGear.PiShock.Devices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BottomGear.PiShock.PiShockApi
{
    public class PiShockClient : IDisposable
    {
        private HttpClient HttpClient { get; set; }
        private static readonly string PiShockApiEndpoint = "https://do.pishock.com/api/apioperate";

        public PiShockClient()
        {
            HttpClient = new HttpClient();
        }

        public void Shock(PiShockDevice device)
        {
            var config = PiShockConfigProvider.Config;

            string request = "{\"Username\":\"" + config.Username
            + "\",\"Apikey\":\"" + config.ApiKey
            + "\",\"Name\":\"" + device.LogName
            + "\",\"Code\":\"" + device.ShareCode
            + "\",\"Intensity\":\"" + device.Strength
            + "\",\"Duration\":\"" + device.Duration
            + "\",\"Op\":\"" + (int)device.ShockType + "\"}";

            Task.Run(async () =>
            {
                await HttpClient.PostAsync(PiShockApiEndpoint, new StringContent(request, Encoding.UTF8, "application/json"));
            }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine("Error while trying to send API request:");
                    Console.WriteLine(t.Exception);
                }
            });
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }
}
