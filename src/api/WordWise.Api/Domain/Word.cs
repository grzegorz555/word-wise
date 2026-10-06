namespace WordWise.Api.Domain;

// One form of a Concept in one language, e.g. Concept "house" → Word(pl, "dom") (ADR-0005).
// Synonyms are several Words with the same ConceptId and Language.
public sealed class Word
{
    public int Id { get; set; }

    // TODO(human): add ConceptId (FK), Concept (reference navigation), Language ("en"/"pl"/"tr"),
    // Text and FrequencyRank (nullable: null = outside the top 1000).
    // hint: a non-nullable FK makes the relationship required; the navigation is `= null!;`, because EF fills it.
    // Concept "Nullable reference types in EF": https://learn.microsoft.com/ef/core/miscellaneous/nullable-reference-types
}
