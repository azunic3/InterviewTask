namespace InterviewTask.Dtos
{
    public class DrugSearchResponseDto
    {
        public string Query { get; set; } = "";
        public string DrugKey { get; set; } = "";

        public DrugInfoDto Drug { get; set; } = new();

        public string SafetyText { get; set; } = "";

        public bool HasPossibleAllergyMatch { get; set; }
        public List<string> MatchedTerms { get; set; } = new();

        public string AvailabilityStatus { get; set; } = "Unknown"; // Available | NotAvailable | Unknown
    }

    public class DrugInfoDto
    {
        public string? BrandName { get; set; }
        public string? GenericName { get; set; }
        public string? ManufacturerName { get; set; }
        public string? ActiveIngredient { get; set; }
    }
}
