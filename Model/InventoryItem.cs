using InterviewTask.Enums;

namespace InterviewTask.Model
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string DrugKey { get; set; } = null!;
        public AvailabilityStatus Status { get; set; } = AvailabilityStatus.NotAvailable;
        public int Quantity { get; set; } = 0;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
