using ITSRunning.DataAccess.Activities;
using ITSRunning.DataAccess.Runners;
using ITSRunning.DataAccess.Telemetries;
using ITSRunning.Models.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ITSRunning.Worker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                "appsettings.json",
                optional: true,
                reloadOnChange: true);
            var configuration = builder.Build();

            string sqlConnectionString = configuration["ConnectionStrings:SqlDb"];
            IActivityRepository activityRepository = new ActivityRepository(sqlConnectionString);
            IRunnerRepository runnerRepository = new RunnerRepository(sqlConnectionString);
            ITelemetryRepository telemetryRepository = new TelemetryRepository(sqlConnectionString);

            string serviceBusConnectionString = configuration["ConnectionStrings:ServiceBus"];
            string queueName = configuration["ServiceBusQueueName"];
            IQueueClient queueClient = new QueueClient(serviceBusConnectionString, queueName);
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(
                async (message, cancellationToken) =>
                {
                    var bytes = message.Body;
                    var json = Encoding.UTF8.GetString(bytes);
                    var content = JsonConvert.DeserializeObject<JObject>(json);
                    var requestType = content.Value<string>("RequestType");
                    Boolean completed = false;

                    switch (requestType)
                    {
                        case "NewTrainingRequest":
                            {
                                var runnerUsername = content.Value<string>("RunnerUsername");
                                Runner runner = runnerRepository.GetByUsername(runnerUsername);
                                var activity = new Activity()
                                {
                                    Name = content.Value<string>("Name"),
                                    IdRunner = runner.Id,
                                    CreationDate = content.Value<DateTime>("Date"),
                                    Location = content.Value<string>("Location"),
                                    UriMatch = content.Value<string>("UriMatch"),
                                    Type = 1,
                                    State = 0
                                };
                                activityRepository.Insert(activity);
                                completed = true;
                                break;
                            }
                        case "StartTrainingRequest":
                            {
                                var idActivity = content.Value<int>("IdActivity");
                                var activity = activityRepository.Get(idActivity);
                                activity.State = 1;
                                activityRepository.Update(activity);
                                completed = true;
                                break;
                            }
                        /*case "ListTrainingRequest":
                            {
                                var runnerUsername = content.Value<string>("RunnerUsername");
                                var response = new ListTrainingResponse()
                                {
                                    Activities = activityRepository.GetByTypeAndUsername(1, runnerUsername),
                                    RunnerUsername = runnerUsername
                                };
                                var notificationJson = JsonConvert.SerializeObject(response);
                                var notificationBytes = Encoding.UTF8.GetBytes(notificationJson);
                                var topicMessage = new Message(notificationBytes);

                                string serviceBusTopicConnectionString = configuration["ConnectionStrings:ServiceBusTopic"];
                                string topicName = configuration["ServiceBusTopicName"];
                                var topicClient = new TopicClient(serviceBusTopicConnectionString, topicName);
                                await topicClient.SendAsync(topicMessage);
                                await topicClient.CloseAsync();
                                completed = true;
                                break;
                            }*/
                        case "DeleteActivityRequest":
                            {
                                var idActivity = content.Value<int>("IdActivity");
                                activityRepository.Delete(idActivity);
                                completed = true;
                                break;
                            }
                        case "UpdateSelfieRequest":
                            {
                                var uriPic = content.Value<string>("UriPic");
                                var idActivity = content.Value<int>("IdActivity");
                                var instant = content.Value<DateTime>("Instant");
                                telemetryRepository.Update(uriPic, idActivity, instant);
                                completed = true;
                                break;
                            }
                        default:
                            await queueClient.AbandonAsync(message.SystemProperties.LockToken);
                            break;
                    }
                    if (completed)
                    {
                        await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                    } else
                    {
                        await queueClient.AbandonAsync(message.SystemProperties.LockToken);
                    }

                }, messageHandlerOptions);
            //insert an option to don't make the worker always active with no messages in the queue
            await Task.Delay(Timeout.Infinite);
            //await queueClient.CloseAsync();
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
