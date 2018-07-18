using ITSRunning.Models.Telemetries;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;

namespace ITSRunning.WebApp.Services
{
    public class TelemetrySender : ITelemetrySender, IDisposable
    {
        private string ehConnectionString;
        private string ehEntityPath;
        private static EventHubClient eventHubClient;
        private EventHubsConnectionStringBuilder connectionStringBuilder;
        private readonly IConfiguration _configuration;

        public TelemetrySender(IConfiguration configuration)
        {
            _configuration = configuration;
            ehConnectionString = _configuration["ConnectionStrings:EventHub"];
            ehEntityPath = _configuration["EventHubName"];
            connectionStringBuilder = new EventHubsConnectionStringBuilder(ehConnectionString)
            {
                EntityPath = ehEntityPath
            };
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
        }

        public async void SendMessage(TelemetryData telemetryData)
        {
            try
            {
                var message = JsonConvert.SerializeObject(telemetryData);
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
            }
            catch (Exception exception)
            {

                throw exception;
            }


        }

        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                eventHubClient.Close();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}
