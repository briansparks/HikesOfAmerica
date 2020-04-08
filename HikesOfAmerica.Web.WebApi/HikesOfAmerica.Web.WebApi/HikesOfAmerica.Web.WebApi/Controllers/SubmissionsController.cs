using HikesOfAmerica.Core.DataModels;
using HikesOfAmerica.Core.Interfaces;
using HikesOfAmerica.Core.Requests;
using HikesOfAmerica.Web.WebApi.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace HikesOfAmerica.Web.WebApi.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private ISubmissionsManager submissionsManager;

        public SubmissionsController(ISubmissionsManager argSubmissionsManager)
        {
            submissionsManager = argSubmissionsManager;
        }

        [HttpPost]
        public IActionResult SubmitNewLocation([FromForm] LocationRequest request)
        {
            try
            {
                request.file = Helpers.GetFormParam<IFormFile>(Request, "file");
                request.trails = Helpers.GetListParam<Trail>(Request, "trails");
                var result = submissionsManager.TrySubmitNewLocation(request);

                if (!result)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"failed to submit new location: {request}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveLocationRequest(string id)
        {
            try
            {
                await submissionsManager.ApproveLocationRequest(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"failed to approve location request id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
