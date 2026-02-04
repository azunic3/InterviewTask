using System.Text.Json;
using InterviewTask.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrugsController : ControllerBase
{
    private readonly OpenFDAService _openFda;

    public DrugsController(OpenFDAService openFda)
    {
        _openFda = openFda;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is required.");

        var doc = await _openFda.SearchDrugLabelRawAsync(query, limit: 3, ct);

        var preview = BuildPreview(doc.RootElement);

        // doc se može dispose-ati odmah jer ništa iz njega više ne vraćaš
        doc.Dispose();

        return Ok(new
        {
            query,
            preview
        });
    }


    private static object BuildPreview(JsonElement root)
    {
        // openFDA obično vraća: { meta: {...}, results: [ {...}, ... ] }
        if (!root.TryGetProperty("results", out var results) || results.ValueKind != JsonValueKind.Array)
            return new { found = 0, items = Array.Empty<object>() };

        var items = results.EnumerateArray()
            .Take(3)
            .Select(r => new
            {
                // Ova polja često postoje, ali ne uvijek - zato TryGet
                brand_name = GetFirstString(r, "openfda", "brand_name"),
                generic_name = GetFirstString(r, "openfda", "generic_name"),
                manufacturer_name = GetFirstString(r, "openfda", "manufacturer_name"),
                active_ingredient = GetFirstString(r, "active_ingredient"),
                warnings = GetFirstString(r, "warnings"),
                contraindications = GetFirstString(r, "contraindications")
            })
            .ToList();

        return new { found = items.Count, items };
    }

    private static string? GetFirstString(JsonElement obj, params string[] path)
    {
        JsonElement cur = obj;

        foreach (var p in path)
        {
            if (!cur.TryGetProperty(p, out cur))
                return null;
        }

        // Polje može biti string ili array stringova
        return cur.ValueKind switch
        {
            JsonValueKind.String => cur.GetString(),
            JsonValueKind.Array => cur.EnumerateArray().FirstOrDefault().GetString(),
            _ => null
        };
    }
}
