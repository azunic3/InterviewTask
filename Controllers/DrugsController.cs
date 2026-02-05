using InterviewTask.Data;
using InterviewTask.Dtos;
using InterviewTask.Model;
using InterviewTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InterviewTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrugsController : ControllerBase
{
    private readonly OpenFDAService _openFda;
    private readonly InventoryService _inventory;
    private readonly AppDbContext _db;

    public DrugsController(OpenFDAService openFda, InventoryService inventory, AppDbContext db)
    {
        _openFda = openFda;
        _inventory = inventory;
        _db = db;
    }

    [HttpGet("search")]
    public async Task<ActionResult<DrugSearchResponseDto>> Search(
        [FromQuery] string query,
        [FromQuery] string? allergens,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is required.");

        using var doc = await _openFda.SearchDrugLabelRawAsync(query, limit: 3, ct);

        if (!doc.RootElement.TryGetProperty("results", out var results) || results.ValueKind != JsonValueKind.Array)
            return NotFound("No results from openFDA.");

        var first = results.EnumerateArray().FirstOrDefault();
        if (first.ValueKind == JsonValueKind.Undefined)
            return NotFound("No results from openFDA.");

        var brand = GetFirstString(first, "openfda", "brand_name");
        var generic = GetFirstString(first, "openfda", "generic_name");
        var manufacturer = GetFirstString(first, "openfda", "manufacturer_name");

        var activeIngredient = GetFirstString(first, "active_ingredient");
        var warnings = GetFirstString(first, "warnings");
        var contraindications = GetFirstString(first, "contraindications");

        var safetyText = string.Join("\n\n",
            new[] { activeIngredient, warnings, contraindications }
                .Where(x => !string.IsNullOrWhiteSpace(x)));

        var drugKey = DrugKeyBuilder.Build(generic, brand, query);

        var inventoryItem = await _inventory.GetOrCreateAsync(drugKey, ct);

        var matched = AllergyMatcher.GetMatchedTerms(safetyText, allergens);

        var dto = new DrugSearchResponseDto
        {
            Query = query.Trim(),
            DrugKey = drugKey,
            Drug = new DrugInfoDto
            {
                BrandName = brand,
                GenericName = generic,
                ManufacturerName = manufacturer,
                ActiveIngredient = activeIngredient
            },
            SafetyText = safetyText,
            MatchedTerms = matched,
            HasPossibleAllergyMatch = matched.Count > 0,
            AvailabilityStatus = inventoryItem.Status.ToString()
        };

        var log = new AllergyCheckLog
        {
            Query = query.Trim(),
            DrugKey = drugKey,
            AllergensRaw = allergens ?? "",
            ResultJson = JsonSerializer.Serialize(new
            {
                matchedTerms = matched,
                hasMatch = matched.Count > 0
            }),
            CheckedAtUtc = DateTime.UtcNow
        };

        _db.AllergyCheckLogs.Add(log);
        await _db.SaveChangesAsync(ct);

        return Ok(dto);
    }

    [HttpGet("popular")]
    public async Task<ActionResult<List<PopularSearchesDto>>> GetPopularSearches(
    [FromQuery] int limit = 5,
    CancellationToken ct = default)
    {
        if (limit <= 0 || limit > 20)
            limit = 5;

        var data = await _db.AllergyCheckLogs
            .AsNoTracking()
            .GroupBy(x => x.Query)
            .Select(g => new PopularSearchesDto
            {
                Query = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(limit)
            .ToListAsync(ct);

        return Ok(data);
    }

    [HttpGet("top-adverse-events")]
    public async Task<ActionResult<TopAdverseEventsDto>> GetTopAdverseEvents(
    [FromQuery] string query,
    [FromQuery] int limit = 10,
    CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is required.");

        using var doc = await _openFda.GetTopAdverseReactionsRawAsync(query, limit, ct);

        if (!doc.RootElement.TryGetProperty("results", out var results) ||
            results.ValueKind != JsonValueKind.Array)
            return NotFound("No adverse event data found.");

        var items = new List<AdverseEventItemDto>();

        foreach (var r in results.EnumerateArray())
        {
            var term = r.TryGetProperty("term", out var t) ? t.GetString() : null;
            var count = r.TryGetProperty("count", out var c) && c.TryGetInt32(out var n) ? n : 0;

            if (!string.IsNullOrWhiteSpace(term))
                items.Add(new AdverseEventItemDto
                {
                    Term = term!,
                    Count = count
                });
        }

        var dto = new TopAdverseEventsDto
        {
            DrugKey = query.Trim().ToLowerInvariant(),
            Items = items
        };

        return Ok(dto);
    }

    private static string? GetFirstString(JsonElement obj, params string[] path)
    {
        JsonElement cur = obj;

        foreach (var p in path)
        {
            if (!cur.TryGetProperty(p, out cur))
                return null;
        }

        return cur.ValueKind switch
        {
            JsonValueKind.String => cur.GetString(),
            JsonValueKind.Array => cur.EnumerateArray().FirstOrDefault().GetString(),
            _ => null
        };
    }
}
