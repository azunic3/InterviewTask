using InterviewTask.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace InterviewTask.Services
{
    public class OpenFDAService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OpenFDAOptions _options;

        public OpenFDAService(IHttpClientFactory httpClientFactory, IOptions<OpenFDAOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public async Task<JsonDocument> SearchDrugLabelRawAsync(string query, int limit = 5, CancellationToken ct = default)
        {
            var q = query.Trim().ToLowerInvariant();

            // search ide po brandu i generic name da bude preciznije i manje rezultata
            var search =
                $"openfda.brand_name:\"{q}\" OR " +
                $"openfda.generic_name:\"{q}\" OR " +
                $"active_ingredient:\"{q}\"";

            var url = $"/drug/label.json?search={Uri.EscapeDataString(search)}&limit={limit}";

            if (!string.IsNullOrWhiteSpace(_options.ApiKey))
                url += $"&api_key={Uri.EscapeDataString(_options.ApiKey!)}";

            var client = _httpClientFactory.CreateClient("OpenFda");
            using var resp = await client.GetAsync(url, ct);
            resp.EnsureSuccessStatusCode();

            var stream = await resp.Content.ReadAsStreamAsync(ct);
            return await JsonDocument.ParseAsync(stream, cancellationToken: ct);
        }
    }
}
