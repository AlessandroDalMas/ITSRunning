using ITSRunning.Models.Telemetries;

namespace ITSRunning.WebApp.Services
{
    public interface ITelemetrySender
    {
        void SendMessage(TelemetryData telemetryData);
    }
}