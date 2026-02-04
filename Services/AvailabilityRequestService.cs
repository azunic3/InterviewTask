using InterviewTask.Data;
using InterviewTask.Model;
using Microsoft.EntityFrameworkCore;

namespace InterviewTask.Services
{
    public class AvailabilityRequestService
    {
        private readonly AppDbContext _db;

        public AvailabilityRequestService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(bool created, AvailabilityRequest request)> CreateAsync(string drugKey, string email)
        {
            drugKey = drugKey.Trim().ToLowerInvariant();
            email = email.Trim().ToLowerInvariant();

            var existing = await _db.AvailabilityRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.DrugKey == drugKey &&
                    x.Email == email &&
                    x.Status == AvailabilityRequestStatus.Pending);

            if (existing != null)
            {
                return (false, existing);
            }

            var req = new AvailabilityRequest
            {
                DrugKey = drugKey,
                Email = email,
                Status = AvailabilityRequestStatus.Pending,
                CreatedAtUtc = DateTime.UtcNow
            };

            _db.AvailabilityRequests.Add(req);
            await _db.SaveChangesAsync();

            return (true, req);
        }
    }
}
