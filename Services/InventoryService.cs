using InterviewTask.Data;
using InterviewTask.Enums;
using InterviewTask.Model;
using Microsoft.EntityFrameworkCore;

namespace InterviewTask.Services;

public class InventoryService
{
    private readonly AppDbContext _db;

    public InventoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<InventoryItem> GetOrCreateAsync(string drugKey, CancellationToken ct = default)
    {
        drugKey = (drugKey ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(drugKey))
            throw new ArgumentException("drugKey is required", nameof(drugKey));

        var item = await _db.InventoryItems.FirstOrDefaultAsync(x => x.DrugKey == drugKey, ct);

        if (item != null)
            return item;

        item = new InventoryItem
        {
            DrugKey = drugKey,
            Status = AvailabilityStatus.NotAvailable, 
            Quantity = 0,
            LastUpdated = DateTime.UtcNow
        };

        _db.InventoryItems.Add(item);
        await _db.SaveChangesAsync(ct);

        return item;
    }

    public async Task<AvailabilityStatus> GetStatusAsync(string drugKey, CancellationToken ct = default)
    {
        drugKey = (drugKey ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(drugKey))
            return AvailabilityStatus.Unknown;

        var item = await _db.InventoryItems.AsNoTracking()
            .FirstOrDefaultAsync(x => x.DrugKey == drugKey, ct);

        return item?.Status ?? AvailabilityStatus.NotAvailable; 
    }
}
