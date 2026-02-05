namespace InterviewTask.Dtos
{
    public class SimilarDrugsResponseDto
    {
        public string Query { get; set; } = null!;
        public string? IngredientKey { get; set; }
        public List<SimilarDrugDto> Items { get; set; } = new();
    }
}
