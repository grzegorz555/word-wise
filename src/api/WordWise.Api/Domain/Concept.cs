namespace WordWise.Api.Domain;

// One meaning shared across languages, e.g. "house" or "right_direction" (ADR-0005).
// A translation = the Words that point to the same Concept.
// Docs: https://learn.microsoft.com/ef/core/modeling/entity-types
public sealed class Concept
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;
    public ICollection<Word> Words { get; } = new List<Word>();
}
