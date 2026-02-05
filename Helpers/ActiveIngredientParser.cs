using System.Text.RegularExpressions;

public static class ActiveIngredientParser
{
    public static string? ExtractPrimaryIngredientKey(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return null;

        var text = raw.ToLowerInvariant();

        // samo ukloni poznate fraze, ali NE briši do kraja reda
        text = text.Replace("drug facts", "");
        text = text.Replace("active ingredient", "");
        text = text.Replace("active ingredients", "");
        text = text.Replace("(in each tablet)", "");
        text = text.Replace("(in each 5 ml teaspoonful)", "");

        // sad tražimo "ime + doza"
        var match = Regex.Match(
            text,
            @"\b([a-z][a-z\s\-]+?)\s+(hcl\s+)?\d+(\.\d+)?\s*(mg|ml|mcg)\b",
            RegexOptions.IgnoreCase
        );

        if (!match.Success)
            return null;

        var ingredient = match.Groups[1].Value.Trim();

        // makni salt forms ako su se provukli
        ingredient = Regex.Replace(
            ingredient,
            @"\b(hcl|hydrochloride|hbr|sulfate|phosphate|sodium|potassium)\b",
            "",
            RegexOptions.IgnoreCase
        );

        ingredient = Regex.Replace(ingredient, @"\s+", " ").Trim();
        ingredient = Regex.Replace(ingredient, @"^(s\s+)", "", RegexOptions.IgnoreCase).Trim();


        return string.IsNullOrWhiteSpace(ingredient) ? null : ingredient;
    }
}
