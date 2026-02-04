using Microsoft.AspNetCore.Mvc;

namespace InterviewTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugsController : ControllerBase
    {
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required");

            return Ok(new
            {
                name = query,
                message = "Backend radiiiiiii"
            });
        }
    }
}
