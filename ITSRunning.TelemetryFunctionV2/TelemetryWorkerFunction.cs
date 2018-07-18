using ITSRunning.DataAccess.Telemetries;
using ITSRunning.Models.Models;
using ITSRunning.Models.Telemetries;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Newtonsoft.Json;
using System;

namespace ITSRunning.TelemetryFunctionV2
{
    public static class TelemetryWorkerFunction
    {
        [FunctionName("TelemetryWorkerFunction")]
        public static void Run([EventHubTrigger("itsrunningeventhub", Connection = "EventHubCS")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
            var message = JsonConvert.DeserializeObject<TelemetryData>(myEventHubMessage);

            string cs = Environment.GetEnvironmentVariable("SqlConnectionString");
            var db = new TelemetryRepository(cs);
            var telemetry = new Telemetry()
            {
                Latitude = message.Latitude,
                Longitude = message.Longitude,
                Instant = message.Instant,
                IdActivity = message.IdActivity
            };
            db.Insert(telemetry, message.Username);
        }
    }
}
