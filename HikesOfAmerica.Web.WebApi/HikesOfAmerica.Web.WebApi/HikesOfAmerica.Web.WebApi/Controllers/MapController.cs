using HikesOfAmerica.Core.DataModels;
using HikesOfAmerica.Core.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HikesOfAmerica.Web.WebApi.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private ILocationsManager locationsManager;

        public MapController(ILocationsManager argLocationsManager)
        {
            locationsManager = argLocationsManager;
        }

        [HttpGet("locations")]
        public async Task<ActionResult<List<Location>>> GetLocations()
        {
            var result = new List<Location>();

            try
            {
                result = await locationsManager.GetLocationsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get locations.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("location/{locationName}")]
        public async Task<ActionResult<List<Location>>> GetLocationByName(string locationName)
        {
            try
            {
                var result = await locationsManager.GetLocationsAsync();

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "failed to get location by name.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("locations")]
        public async Task<ActionResult<string>> AddLocation(Location location)
        {
            try
            {
                if (location == null)
                    return BadRequest();

                var result = await locationsManager.AddLocationAsync(location);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"failed to add location: ${location?.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
