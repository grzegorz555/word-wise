using System.Globalization;

namespace WordWise.Api.Domain;

// The languages WordWise supports, as ISO 639-1 codes (ADR-0006). Word.Language holds one of these.
// `const` fields can be used in attributes, e.g. [AllowedValues(LanguageCodes.English, ...)].
// Docs: https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/const
public static class LanguageCodes
{
    public const string English = "en";
    public const string Polish = "pl";
    public const string Turkish = "tr";

    // The culture that sorts and compares text in a given language (Turkish İ/ı, Polish ą/ł, ADR-0006).
    // GetCultureInfo returns a cached, read-only instance, so calling it per request is cheap.
    // Docs: https://learn.microsoft.com/dotnet/api/system.globalization.cultureinfo.getcultureinfo
    public static CultureInfo GetCulture(string languageCode) => CultureInfo.GetCultureInfo(languageCode);
}
