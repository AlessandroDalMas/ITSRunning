using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSRunning.WebApp.Services
{
    public class SignalRRegistry : ISignalRRegistry
    {
        private ConcurrentDictionary<string, List<string>> _usernamesToClientIds = new ConcurrentDictionary<string, List<string>>();

        public List<string> ClientIdFromUsername(string username)
        {
            if (_usernamesToClientIds.ContainsKey(username))
                return _usernamesToClientIds[username];
            else
                return new List<string>();
        }

        public void Register(string clientId, string username)
        {
            if (_usernamesToClientIds.ContainsKey(username))
            {
                var connections = _usernamesToClientIds[username];
                if (!connections.Contains(clientId))
                    connections.Add(clientId);
            }
            else
            {
                var value = new List<string>()
                {
                    clientId
                };
                _usernamesToClientIds.AddOrUpdate(username, value, (key, oldValue) => value);
            }
        }
    }
}
