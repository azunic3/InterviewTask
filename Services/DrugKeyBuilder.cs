namespace InterviewTask.Services
{
    public static class DrugKeyBuilder
    {
        public static string Build(string? genericName, string? brandName, string query)
        {
            string key = genericName ?? brandName ?? query;
            key = key.Trim().ToLowerInvariant();
            return string.IsNullOrWhiteSpace(key) ? query.Trim().ToLowerInvariant() : key;
        }
    }
}
