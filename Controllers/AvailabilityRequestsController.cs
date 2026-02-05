using InterviewTask.Dtos;
using InterviewTask.Services;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;

namespace InterviewTask.Controllers
{
    [ApiController]
    [Route("api/availability-requests")]
    public class AvailabilityRequestsController : ControllerBase
    {
        private readonly AvailabilityRequestService _service;
        private readonly IConfiguration _config;

        public AvailabilityRequestsController(AvailabilityRequestService service, IConfiguration config)
        {
            _service = service;
            _config = config;

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
        [HttpPost("notify")]
        public async Task<IActionResult> Notify([FromBody] CreateAvailabilityRequestDto dto)
        {
            var apiKey = _config["Mailgun:ApiKey"];
            var domain = _config["Mailgun:Domain"];
            var from = _config["Mailgun:From"];
            var baseUrl = _config["Mailgun:BaseUrl"]; 

            if (string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrWhiteSpace(domain) ||
                string.IsNullOrWhiteSpace(from) ||
                string.IsNullOrWhiteSpace(baseUrl))
                return StatusCode(500, "Mailgun config missing.");

            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator("api", apiKey)
            };

            var client = new RestClient(options);
            var request = new RestRequest($"/v3/{domain}/messages", Method.Post);
            request.AlwaysMultipartFormData = true;

            request.AddParameter("from", from);
            request.AddParameter("to", dto.Email);
            request.AddParameter("subject", $"Zahtjev zaprimljen – {dto.DrugKey}");
            request.AddParameter("text", "Hvala! Zaprimili smo vaš zahtjev. Obavijestit ćemo vas kada bude dostupno.");

            var response = await client.ExecuteAsync(request);

            return StatusCode((int)response.StatusCode,
                response.Content ?? response.ErrorMessage ?? "No response content");
        }

    }
}
