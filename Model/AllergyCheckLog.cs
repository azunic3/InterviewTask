namespace InterviewTask.Model
{
    public class AllergyCheckLog
    {
        public int Id { get; set; }

        public string Query { get; set; } = null!;      
        public string DrugKey { get; set; } = null!;

        public string AllergensRaw { get; set; } = null!;

        public string ResultJson { get; set; } = null!; 
        public DateTime CheckedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
