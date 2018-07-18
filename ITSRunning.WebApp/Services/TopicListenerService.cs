using ITSRunning.Models.Responses;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ITSRunning.WebApp.Services
{
    public class TopicListenerService : IHostedService
    {
        private IConfiguration _configuration;
        private IHubContext<TopicListenerHub> _hub;
        private ISignalRRegistry _registry;

        public TopicListenerService(IConfiguration configuration, IHubContext<TopicListenerHub> hub, ISignalRRegistry registry)
        {
            _configuration = configuration;
            _hub = hub;
            _registry = registry;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            string serviceBusTopicConnectionString = _configuration["ConnectionStrings:ServiceBusTopic"];
            string topicName = _configuration["ServiceBusTopicName"];
            string _subscriptionName = _configuration["ServiceBusTopicSubscriptionName"];
            SubscriptionClient subscriptionClient = new SubscriptionClient(serviceBusTopicConnectionString, topicName, _subscriptionName);
            subscriptionClient.RegisterMessageHandler(
                async (m, c) =>
                {
                    var bytes = m.Body;
                    var json = Encoding.UTF8.GetString(bytes);
                    var content = JsonConvert.DeserializeObject<ListTrainingResponse>(json);
                    var type = content.ResponseType;
                    var username = content.RunnerUsername;

                    switch (type)
                    {
                        case "ListTrainingResponse":
                            await _hub.Clients.Clients(_registry.ClientIdFromUsername(username)).SendAsync("ListTrainingResponse", content.Activities);
                            break;
                        default:
                            break;
                    }
                    await subscriptionClient.CompleteAsync(m.SystemProperties.LockToken);
                },
            new Func<ExceptionReceivedEventArgs, Task>(async (e) =>
            {

            }));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
