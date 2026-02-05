namespace InterviewTask.Dtos
{
    public class DrugUsageDto
    {
        public string Query { get; set; } = null!;
        public string DrugKey { get; set; } = null!;
        public string? UsageText { get; set; }
        public bool HasUsageInfo { get; set; }
    }
}
