namespace WordWise.Api.Domain;

// One form of a Concept in one language, e.g. Concept "house" → Word(pl, "dom") (ADR-0005).
// Synonyms are several Words with the same ConceptId and Language.
public sealed class Word
{
    public int Id { get; set; }
    public int ConceptId { get; set; }
    public Concept Concept { get; set; } = null!;
    public string Language { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public int? FrequencyRank { get; set; }
}
