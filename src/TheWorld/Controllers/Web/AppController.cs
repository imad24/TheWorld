using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Networking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.ViewModels;
using TheWorld.Services;


namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IConfigurationRoot _config;
        private readonly IWorldRepository _repository;
        private ILogger<AppController> _logger;

        public AppController(IMailService mailService,
            IConfigurationRoot config,
            IWorldRepository repository,
            ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Trips()
        {
            try
            {
                return View();
        }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to get trips in Index page : {ex.Message}");
                return Redirect("/error");
    }
}

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("bar.com")) ModelState.AddModelError("Email","Yaw dir addresse riyah matetmanyak");
            if (ModelState.IsValid)
            {
                _mailService.SendEmail(_config["MailSettings:ToAddress"], "From iMad", model.Name, model.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent !";
            }


            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
