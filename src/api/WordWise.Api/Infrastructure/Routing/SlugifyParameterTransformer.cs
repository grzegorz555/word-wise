using System.Text.RegularExpressions;

namespace WordWise.Api.Infrastructure.Routing;

// Turns route tokens into lowercase kebab-case: [controller] "Words" → "words",
// "StudySessions" → "study-sessions". Registered in Program.cs with RouteTokenTransformerConvention,
// so every [Route("api/[controller]")] becomes /api/words, both for routing and in the OpenAPI document.
// The regex has a timeout, as the docs advise for any regex (protection against ReDoS).
// Docs: https://learn.microsoft.com/aspnet/core/mvc/controllers/routing#use-a-parameter-transformer-to-customize-token-replacement
public sealed class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is null)
        {
            return null;
        }

        // "([a-z])([A-Z])": a lowercase letter followed by an uppercase one, e.g. "yS" in "StudySessions".
        return Regex.Replace(
                value.ToString()!,
                "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100))
            .ToLowerInvariant();
    }
}
