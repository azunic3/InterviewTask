namespace InterviewTask.Model
{
    public enum AvailabilityRequestStatus
    {
        Pending = 0,
        ReadyToNotify = 1,
        Closed = 2
    }
    public class AvailabilityRequest
    {
        public int Id { get; set; }

        public string DrugKey { get; set; } = null!;

        public string Email { get; set; } = null!;

        public AvailabilityRequestStatus Status { get; set; } = AvailabilityRequestStatus.Pending;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
