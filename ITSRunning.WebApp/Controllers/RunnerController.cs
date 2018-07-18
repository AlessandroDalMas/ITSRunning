using ITSRunning.Models.Models;
using ITSRunning.Models.Requests;
using ITSRunning.Models.Telemetries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using ITSRunning.WebApp.Services;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ITSRunning.DataAccess.Activities;

namespace ITSRunning.WebApp.Controllers
{

    public class RunnerController : Controller
    {
        private IConfiguration _configuration;
        private readonly ITelemetrySender _telemetrySender;
        private readonly IActivityRepository _activityRepository;
        public RunnerController(ITelemetrySender telemetrySender, IConfiguration configuration, IActivityRepository activityRepository)
        {
            _telemetrySender = telemetrySender;
            _configuration = configuration;
            _activityRepository = activityRepository;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Run(int id)
        {
            try
            {
                var activity = _activityRepository.Get(id);
                if (id != 0)
                {
                    var startActivity = new StartTrainingRequest()
                    {
                        Username = User.Identity.Name,
                        IdActivity = id
                    };
                    await SendCommand(startActivity);
                }

                if (activity.State == 0)
                {
                    ViewData["id"] = id;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index","Training",0);
                }
            }
            catch
            {
                return RedirectToAction("Index", "Training",0);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SendTelemetry([FromBody] BasicTelemetry telemetryData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var telemetry = new TelemetryData()
                    {
                        Latitude = telemetryData.Latitude,
                        Longitude = telemetryData.Longitude,
                        Instant = telemetryData.Instant,
                        Username = User.Identity.Name,
                        IdActivity = telemetryData.IdActivity
                    };
                    _telemetrySender.SendMessage(telemetry);
                }
                catch
                {
                    return BadRequest();
                }
                return Ok();
            } else
            {
                return BadRequest();
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SavePic([FromBody] Selfie selfie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var str = selfie.Pic.Remove(0, selfie.Pic.IndexOf(',') + 1);
                    var arrayBytesPhoto = Convert.FromBase64String(str);
                    var storageAccount = CloudStorageAccount.Parse(_configuration["ConnectionStrings:Storage"]);
                    var blobClient = storageAccount.CreateCloudBlobClient();
                    var camerasContainer = blobClient.GetContainerReference("selfies");
                    await camerasContainer.CreateIfNotExistsAsync();
                    var id = Guid.NewGuid();
                    var fileExtension = ".jpeg";
                    var blobName = $"{selfie.IdActivity}/{id}{fileExtension}";
                    var blobRef = camerasContainer.GetBlockBlobReference(blobName);
                    await blobRef.UploadFromByteArrayAsync(arrayBytesPhoto, 0, arrayBytesPhoto.Length);
                    string sas = blobRef.GetSharedAccessSignature(
                        new SharedAccessBlobPolicy()
                        {
                            Permissions = SharedAccessBlobPermissions.Read
                        });
                    var blobUri = $"{blobRef.Uri.AbsoluteUri}{sas}";
                    var notification = new UpdateSelfieRequest()
                    {
                        UriPic = blobUri,
                        IdActivity = selfie.IdActivity,
                        Instant = selfie.Instant
                    };
                    await SendCommand(notification);
                    return Ok();
                }
                catch
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        public async Task SendCommand<TCommand>(TCommand command)
        {
            string serviceBusConnectionString = _configuration["ConnectionStrings:ServiceBus"];
            string queueName = _configuration["ServiceBusQueueName"];
            IQueueClient queueClient = new QueueClient(serviceBusConnectionString, queueName);
            var messageBody = JsonConvert.SerializeObject(command);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            await queueClient.SendAsync(message);
            await queueClient.CloseAsync();
        }
    }
}