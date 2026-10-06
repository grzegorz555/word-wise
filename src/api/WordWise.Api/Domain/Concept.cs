namespace WordWise.Api.Domain;

// One meaning shared across languages, e.g. "house" or "right_direction" (ADR-0005).
// A translation = the Words that point to the same Concept.
// Docs: https://learn.microsoft.com/ef/core/modeling/entity-types
public sealed class Concept
{
    public int Id { get; set; }

    // TODO(human): add Key (string), PartOfSpeech (string) and the collection navigation Words.
    // hint: non-nullable strings → `= string.Empty;` or the `required` modifier; collection navigation →
    // `public ICollection<Word> Words { get; } = new List<Word>();`
    // Concept "Required one-to-many": https://learn.microsoft.com/ef/core/modeling/relationships/one-to-many#required-one-to-many
}
