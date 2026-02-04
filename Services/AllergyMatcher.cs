namespace InterviewTask.Services
{
    public static class AllergyMatcher
    {
        public static List<string> GetMatchedTerms(string? safetyText, string? allergensCsv)
        {
            if (string.IsNullOrWhiteSpace(safetyText) || string.IsNullOrWhiteSpace(allergensCsv))
                return new List<string>();

            var text = safetyText.ToLowerInvariant();

            var allergens = allergensCsv
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(a => a.ToLowerInvariant())
                .Distinct()
                .ToList();

            var matches = allergens
                .Where(a => a.Length >= 2 && text.Contains(a))
                .ToList();

            return matches;
        }
    }
}
