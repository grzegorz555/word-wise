using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using WordWise.Api.Domain;

namespace WordWise.Api.Features.Words;

// Query string of GET /api/words/cards?base=pl&target=tr&order=sequential (spec 001).
// [ApiController] validates it before the action runs and returns 400 ValidationProblemDetails (R1).
// Docs: https://learn.microsoft.com/aspnet/core/web-api/#automatic-http-400-responses
public sealed record WordCardsQuery : IValidatableObject
{
    // `base` is a C# keyword, so the property has a clearer name and [FromQuery(Name)] maps the query key.
    // Docs: https://learn.microsoft.com/aspnet/core/mvc/models/model-binding#sources
    // [BindRequired]: the key must be in the query string (model binding; also `required: true` in OpenAPI).
    // [Required]: the value must not be empty, e.g. ?base= (model validation).
    // Docs: https://learn.microsoft.com/aspnet/core/mvc/models/model-binding#complex-types
    [FromQuery(Name = "base")]
    [BindRequired]
    [Required]
    [AllowedValues(LanguageCodes.English, LanguageCodes.Polish, LanguageCodes.Turkish)]
    public string BaseLanguage { get; init; } = string.Empty;

    [FromQuery(Name = "target")]
    [BindRequired]
    [Required]
    [AllowedValues(LanguageCodes.English, LanguageCodes.Polish, LanguageCodes.Turkish)]
    public string TargetLanguage { get; init; } = string.Empty;

    // [DefaultValue] only documents the default (`default` in OpenAPI); the initializer sets it at runtime.
    // Docs: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata#use-attributes-to-add-metadata
    [FromQuery(Name = "order")]
    [DefaultValue(CardOrder.Sequential)]
    public CardOrder Order { get; init; } = CardOrder.Sequential;

    // Class-level rule (R1: base ≠ target). It runs only after the attributes above have passed.
    // Docs: https://learn.microsoft.com/aspnet/core/mvc/models/validation#ivalidatableobject
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.Equals(BaseLanguage, TargetLanguage, StringComparison.Ordinal))
        {
            yield return new ValidationResult(
                "The base and target languages must be different.",
                ["target"]);
        }
    }
}
