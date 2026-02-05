namespace InterviewTask.Dtos
{
    public class TopAdverseEventsDto
    {
        public string DrugKey { get; set; } = null!;
        public List<AdverseEventItemDto> Items { get; set; } = new();
    }
}
