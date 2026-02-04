namespace InterviewTask.Model
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string DrugKey { get; set; } = null!;
        public string? DisplayName { get; set; }
        public int Quantity { get; set; } = 0;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
