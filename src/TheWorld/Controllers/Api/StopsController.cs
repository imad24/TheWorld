using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TheWorld.Controllers.Api
{
    [Route("/api/trips/{tripName}/stops")]
    [Authorize]
    public class StopsController : Controller
    {
        private readonly ILogger<StopsController> _logger;
        private readonly IWorldRepository _repository;
        private readonly GeoCoordsService _coordsService;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger,GeoCoordsService coordsService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetUserTripByName(tripName, User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting the trip {ex.Message}");
            }

            return BadRequest("Failed to get stops");

        }

        [HttpPost("")]

        public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel theStop)
        {
            try
            {
                //Test if vm is valid
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(theStop);

                    //lookup geocords

                    var result = await _coordsService.GetGeoCoords(newStop.Name);
                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;

                        //save to db
                        _repository.AddStop(tripName, newStop,User.Identity.Name);

                        if (await _repository.SaveChangesAsynch())
                        {
                            return Created($"/api/trips/{tripName}/stop/{newStop.Name}", Mapper.Map<StopViewModel>(newStop));
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                
               _logger.LogError($"Failed to save new Stop: {ex}");
            }

            return BadRequest("Failed inserting stop");

        }
    }
}
