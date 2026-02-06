using System.Net.Http.Headers;
using System.Text;

namespace InterviewTask.Services;

public class MailgunService
{
    private readonly HttpClient _http;

    public MailgunService(HttpClient http)
    {
        _http = http;
    }

    public async Task SendAsync(string toEmail, string subject, string text)
    {
        var apiKey = Environment.GetEnvironmentVariable("Mailgun__ApiKey");
        var domain = Environment.GetEnvironmentVariable("Mailgun__Domain");
        var from = Environment.GetEnvironmentVariable("Mailgun__From");
        var baseUrl = Environment.GetEnvironmentVariable("Mailgun__BaseUrl") ?? "https://api.mailgun.net";

        if (string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(domain) ||
            string.IsNullOrWhiteSpace(from))
        {
            throw new InvalidOperationException("Mailgun env vars missing: Mailgun__ApiKey / Mailgun__Domain / Mailgun__From");
        }

        _http.BaseAddress = new Uri(baseUrl.TrimEnd('/'));

        var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiKey}"));
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["from"] = from,
            ["to"] = toEmail,
            ["subject"] = subject,
            ["text"] = text
        });

        var res = await _http.PostAsync($"/v3/{domain}/messages", content);
        var body = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
            throw new Exception($"Mailgun failed: {(int)res.StatusCode} {res.ReasonPhrase} | {body}");
    }
}
