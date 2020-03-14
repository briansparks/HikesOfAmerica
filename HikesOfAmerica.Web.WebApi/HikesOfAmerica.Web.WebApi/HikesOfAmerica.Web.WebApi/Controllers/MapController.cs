using HikesOfAmerica.Core.Interfaces;
using HikesOfAmerica.Core.Requests.LocationRequest;
using HikesOfAmerica.Data.Persistence.DataModels;
using HikesOfAmerica.Web.WebApi.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async  Task<ActionResult<List<Location>>> GetLocations()
        {
            var result = await locationsManager.GetLocationsAsync();

            return Ok(result);
        }

        [HttpGet("location/{locationName}")]
        public async Task<ActionResult<List<Location>>> GetLocationByName(string locationName)
        {
            var result = await locationsManager.GetLocationsAsync();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("locations")]
        public async Task<ActionResult<string>> AddLocation(Location location)
        {
            if (location == null)
                return BadRequest();

            var result = await locationsManager.AddLocation(location);

            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(result);
        }

        [HttpPost("location/submit")]
        public IActionResult SubmitNewLocation([FromForm] LocationRequest request)
        {
            request.trails = Helpers.GetListParam<Trail>(Request, "trails");
            var result = locationsManager.TrySubmitNewLocation(request);

            if (!result)
                return BadRequest();

            return Ok();
        }
    }
}
