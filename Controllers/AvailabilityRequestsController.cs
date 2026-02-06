using InterviewTask.Dtos;
using InterviewTask.Services;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace InterviewTask.Controllers
{
    [ApiController]
    [Route("api/availability-requests")]
    public class AvailabilityRequestsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AvailabilityRequestsController(IConfiguration config)
        {
            _config = config;

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
            request.AddParameter("subject", $"Request received – {dto.DrugKey}");
            request.AddParameter("text", "Thank you! We have received your request.We will notify you when it becomes available..");

            var response = await client.ExecuteAsync(request);

            return StatusCode((int)response.StatusCode,
                response.Content ?? response.ErrorMessage ?? "No response content");
        }

    }
}
