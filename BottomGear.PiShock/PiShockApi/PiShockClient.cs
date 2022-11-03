using BottomGear.PiShock.Config;
using BottomGear.PiShock.Devices;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace BottomGear.PiShock.PiShockApi
{
    public class PiShockClient : IPiShockClient
    {
        private readonly HttpClient HttpClient;

        public PiShockClient()
        {
            HttpClient = new HttpClient();
        }

        public void Shock(PiShockDevice device)
        {
            var config = PiShockConfigProvider.Config;

            var shockRequest = new ShockRequest()
            {
                Apikey = config.ApiKey,
                Username = config.Username,
                Code = device.ShareCode,
                Duration = device.Duration.ToString(),
                Intensity = device.Strength.ToString(),
                Name = device.LogName,
                Op = device.ShockType.ToString(),
            };

            string requestBody = JsonSerializer.Serialize(shockRequest);

            if(config.Debug == true)
            {
                Console.WriteLine("Making request:");
                Console.WriteLine(requestBody);
            }

            Task.Run(async () =>
            {
                var result = await HttpClient.PostAsync(config.ApiEndpoint, new StringContent(requestBody, Encoding.UTF8, "application/json"));
                string responseContent = await result.Content.ReadAsStringAsync();
                if (!result.IsSuccessStatusCode || IsApiResponseAnError(responseContent))
                {
                    ExplainApiError(responseContent);
                }
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

        private bool IsApiResponseAnError(string response)
        {
            if (!string.IsNullOrWhiteSpace(response) && (
                response.Equals("Device currently not connected.", StringComparison.OrdinalIgnoreCase)
                || response.Equals("Not Authorized.", StringComparison.OrdinalIgnoreCase)
            ))
            {
                return true;
            }

            return false;
        }

        private void ExplainApiError(string response)
        {
            Console.WriteLine("The Pishock API returned an error!");
            Console.WriteLine("API Error received:");
            Console.WriteLine(response);

            if (response.Equals("Device currently not connected.", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("The API request was successful, but your shocker is not turned on.");
            }
            else if(response.Equals("Not Authorized.", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Error logging into the API. Make sure your Username, API Key and Share Code are all correct. Try generating a new API Key and Share Code.");
            }
        }
    }
}
