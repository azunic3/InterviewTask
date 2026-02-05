namespace InterviewTask.Dtos
{
    public class SimilarDrugDto
    {
        public string DrugKey { get; set; } = null!;
        public string? BrandName { get; set; }
        public string? GenericName { get; set; }
        public string? ManufacturerName { get; set; }
        public string? ActiveIngredientRaw { get; set; }
    }
}
