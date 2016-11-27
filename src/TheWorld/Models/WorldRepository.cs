using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private readonly WorldContext _context;
        private readonly ILogger<WorldContext> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldContext> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting all trips from database");
            return _context.Trips.ToList();
        }

        public IEnumerable<Trip> GetTripsByUserName(string userName)
        {
            _logger.LogInformation("Getting all trips from database");
            return _context
                .Trips
                .Include(t=>t.Stops)
                .Where(t=>t.Username== userName)
                .ToList();
        }

        public Trip GetUserTripByName(string tripName, string userName)
        {
            return _context.Trips
    .Include(t => t.Stops)
    .FirstOrDefault(t => t.Name == tripName && t.Username== userName);
        }

        public Trip GetTripByName(string tripName)
        {
            try
            {
                return _context.Trips
                    .Include(t=>t.Stops)
                    .FirstOrDefault(t => t.Name == tripName);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error when getting trip by name {ex}");
            }

            return null;
        }

        public void AddTrip(Trip trip)
        {
            try
            {
                _context.Add(trip);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when saving changes {ex}");
            }
        }



        public void AddStop(string tripName,Stop stop,string userName)
        {
            try
            {
                var trip = GetUserTripByName(tripName,userName);
                if (trip != null)
                {
                    trip.Stops.Add(stop);
                    _context.Stops.Add(stop);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when saving changes {ex}");
            }
        }

        public async Task<bool> SaveChangesAsynch()
        {
            try
            {
               return (await _context.SaveChangesAsync())>0;
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when saving changes {ex}");
                return false;
            }
        }
    }
}
