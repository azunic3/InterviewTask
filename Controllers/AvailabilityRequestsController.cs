using InterviewTask.Dtos;
using InterviewTask.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTask.Controllers
{
    [ApiController]
    [Route("api/availability-requests")]
    public class AvailabilityRequestsController : ControllerBase
    {
        private readonly AvailabilityRequestService _service;

        public AvailabilityRequestsController(AvailabilityRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAvailabilityRequestDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var (created, request) = await _service.CreateAsync(dto.DrugKey, dto.Email);

            return Ok(new
            {
                created,
                requestId = request.Id,
                drugKey = request.DrugKey,
                email = request.Email,
                status = request.Status.ToString(),
                createdAtUtc = request.CreatedAtUtc
            });
        }
    }
}
