﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        private readonly IWorldRepository _repository;
        private readonly ILogger<TripsController> _logger;

        public TripsController(IWorldRepository repository,ILogger<TripsController> logger )
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]

        public IActionResult Get()
        {
            try
            {
                var result = _repository.GetTripsByUserName(this.User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all trips {ex}");
                return BadRequest("Error occured");
            }

        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(theTrip);
                newTrip.Username = User.Identity.Name;
                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsynch())
                {
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }
            }
            return BadRequest("Failed to save changes to the database");

        }
    }
}
