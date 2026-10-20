using System.Text.Json.Serialization;

namespace WordWise.Api.Features.Words;

// Order of the cards in GET /api/words/cards (spec 001, R2). Sequential is the default (value 0).
// Without [JsonConverter] the OpenAPI schema would describe the enum as an integer; with it, as the strings
// below, so the generated Angular client gets 'sequential' | 'random'. The query string itself is bound
// with Enum.TryParse (case-insensitive), so ?order=random maps to Random either way.
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata#include-openapi-metadata-for-data-types
//   https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/customize-properties#custom-enum-member-names
[JsonConverter(typeof(JsonStringEnumConverter<CardOrder>))]
public enum CardOrder
{
    [JsonStringEnumMemberName("sequential")]
    Sequential,

    [JsonStringEnumMemberName("random")]
    Random,
}
