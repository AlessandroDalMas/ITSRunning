using Microsoft.AspNetCore.SignalR;
using ITSRunning.WebApp.Services;

namespace ITSRunning.WebApp.Services
{
    public class TopicListenerHub : Hub
    {
        private ISignalRRegistry _registry;

        public TopicListenerHub(ISignalRRegistry registry)
        {
            _registry = registry;
        }
        public void Register(string id, string username)
        {
            _registry.Register(id, username);
        }
    }
}
