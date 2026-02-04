namespace InterviewTask.Model
{
    public class DrugLabelCache
    {
        public int Id { get; set; }
        public string SetId { get; set; } = null!;
        public string QueryKey { get; set; } = null!;
        public string? BrandName { get; set; }
        public string? GenericName { get; set; }
        public string? ManufacturerName { get; set; }
        public string JsonLabel { get; set; } = null!;
        public DateTime CachedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
