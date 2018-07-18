using System.Collections.Generic;

namespace ITSRunning.WebApp.Services
{
    public interface ISignalRRegistry
    {
        void Register(string clientId, string username);
        List<string> ClientIdFromUsername(string username);
    }
}