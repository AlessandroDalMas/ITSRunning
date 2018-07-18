using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ITSRunning.WebApp.Models;
using ITSRunning.WebApp.Models.TrainingViewModels;
using ITSRunning.DataAccess.Activities;
using ITSRunning.Models.Requests;
using ITSRunning.DataAccess.Telemetries;
using ITSRunning.Models.Models;
using System.Collections.Generic;

namespace ITSRunning.WebApp.Controllers
{
    public class TrainingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IActivityRepository _activityRepository;
        private readonly ITelemetryRepository _telemetryRepository;

        public TrainingController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IActivityRepository activityRepository,
            ITelemetryRepository telemetryRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _activityRepository = activityRepository;
            _telemetryRepository = telemetryRepository;
        }
        /*
        [Authorize]
        public ActionResult Index()
        {
            var activities = _activityRepository.GetByTypeAndUsername(1, User.Identity.Name);
            return View(activities);
            /* use async method and get response with signalr
            try
            {
                var listTraining = new ListTrainingRequest()
                {
                    RunnerUsername = User.Identity.Name
                };
                await SendCommand(listTraining);
            }
            catch
            {
            }
            return View();
            */
        //}

        [Authorize]
        public ActionResult Index(int id)
        {
            var activities = _activityRepository.GetByTypeAndUsername(1, User.Identity.Name);
            ViewData["id"] = id;
            return View(activities);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var telemetries = _telemetryRepository.GetAll(id);
            return View(telemetries);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Training/Create
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(TrainingCreation training)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTraining = new NewTrainingRequest()
                    {
                        RunnerUsername = User.Identity.Name,
                        Name = training.Name,
                        Location = training.Location,
                        Date = DateTime.Now
                    };
                    await SendCommand(newTraining);
                    return RedirectToAction(nameof(Index),0);
                }
                else
                {
                    return View(training);
                }
            }
            catch
            {
                return View(training);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult StartTraining(int id)
        {
            try
            {
                return RedirectToAction("Run", "Runner", new { id = id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Training/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleteTraining = new DeleteActivityRequest()
                {
                    Username = User.Identity.Name,
                    IdActivity = id
                };
                await SendCommand(deleteTraining);
                return RedirectToAction("Run", "Runner", new { id = id });
            }
            catch
            {
                return RedirectToAction(nameof(Index), new { id = id });
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