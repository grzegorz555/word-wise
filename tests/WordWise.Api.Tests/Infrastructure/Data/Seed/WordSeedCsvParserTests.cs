using WordWise.Api.Infrastructure.Data.Seed;

namespace WordWise.Api.Tests.Infrastructure.Data.Seed;

// Test list for spec 001 / T4 [solo]. Bodies are written by the author (Arrange / Act / Assert).
// Decisions referenced below: docs/specs/001-word-catalog/tasks.md (T4, decisions 1-6).
// Sample row from data/seed/words.seed.csv:
//   concept_key;part_of_speech;en;en_rank;pl;pl_rank;tr;tr_rank
//   say;verb;say;3;mówić|powiedzieć;3;demek|söylemek;3
public sealed class WordSeedCsvParserTests
{
    // ---------- A. Valid input ----------

    // Baseline: header + one simple row. Checks the whole record shape:
    // concept key, part of speech, and en/pl/tr each with forms and rank.
    [Fact(Skip = "TDD red: parser logic not written yet (spec 001 T4). Remove Skip when implementing.")]
    public void Parse_SingleValidRow_ReturnsRecordWithAllLanguages()
    {
        string[] lines = [
            "concept_key;part_of_speech;en;en_rank;pl;pl_rank;tr;tr_rank",
            "say;verb;say;3;mówić|powiedzieć;3;demek|söylemek;3"
        ];

        var result = WordSeedCsvParser.ParseWordSeed(lines);

        result.Count.ShouldBe(1);
    }

    // Acceptance criterion: "a|b" synonyms. Forms keep the order from the file.
    [Fact]
    public void Parse_PipeSeparatedForms_ReturnsEachFormInFileOrder()
    {
    }

    // Complement of the test above: no "|" means a list with exactly one form.
    [Fact]
    public void Parse_SingleForm_ReturnsOneElementList()
    {
    }

    // Acceptance criterion + ADR-0005: an empty rank is null, other languages keep their numbers.
    [Fact]
    public void Parse_EmptyRank_ReturnsNullRankForThatLanguageOnly()
    {
    }

    // Convention: no ToLower()/culture-sensitive changes. Use ł, ş, İ, ı and mixed case;
    // the forms must come back exactly as written.
    [Fact]
    public void Parse_PolishAndTurkishCharacters_KeepsFormsUnchanged()
    {
    }

    // Decision 4: columns are mapped by header name, not by position.
    // Example header: concept_key;part_of_speech;tr;tr_rank;en;en_rank;pl;pl_rank
    [Fact]
    public void Parse_LanguageColumnsInDifferentOrder_MapsLanguagesByHeader()
    {
    }

    // Decision 5: whitespace around ";" is trimmed (e.g. "say ; verb ; ...").
    [Fact]
    public void Parse_WhitespaceAroundSemicolons_TrimsValues()
    {
    }

    // Decision 5: whitespace around "|" is trimmed (" dom | chata " -> "dom", "chata").
    [Fact]
    public void Parse_WhitespaceAroundPipes_TrimsForms()
    {
    }

    // Many rows: one record per row, same order as in the file.
    [Fact]
    public void Parse_MultipleRows_ReturnsOneRecordPerRowInFileOrder()
    {
    }

    // ---------- B. Invalid input (decision 3: fail fast, message names line number + reason) ----------
    // Line numbers: the header is line 1, so the first data row is line 2 (same as in an editor).

    // Decision 4: a language column without its "xx_rank" pair is an error in the header.
    [Fact]
    public void Parse_HeaderMissingRankColumn_Throws()
    {
    }

    // Decision 3: a row with fewer columns than the header.
    [Fact]
    public void Parse_RowWithMissingColumns_ThrowsWithLineNumber()
    {
    }

    // Decision 3 + CA1305: rank must be an integer (parsed with CultureInfo.InvariantCulture).
    [Fact]
    public void Parse_NonNumericRank_ThrowsWithLineNumber()
    {
    }

    // Decision 6: an empty synonym between two pipes.
    [Fact]
    public void Parse_EmptySynonymBetweenPipes_ThrowsWithLineNumber()
    {
    }

    // Decision 6: a trailing pipe also creates an empty synonym.
    // Could be merged with the test above into one [Theory] with [InlineData].
    [Fact]
    public void Parse_TrailingPipe_ThrowsWithLineNumber()
    {
    }

    // ---------- C. PROPOSALS: not covered by decisions 1-6 yet ----------
    // The author decides each case, records it in tasks.md (T4), then keeps, renames or deletes the test.

    // Proposal: no lines at all = no header = error.
    [Fact]
    public void Parse_NoLines_Throws()
    {
    }

    // Proposal: header only is valid and gives an empty result.
    [Fact]
    public void Parse_HeaderOnly_ReturnsEmptyList()
    {
    }

    // Proposal: blank lines (e.g. a trailing newline added by an editor) are skipped.
    [Fact]
    public void Parse_BlankLine_SkipsIt()
    {
    }

    // Proposal: an empty forms column is a bad row (every concept needs all three languages).
    [Fact]
    public void Parse_EmptyFormsColumn_ThrowsWithLineNumber()
    {
    }

    // Proposal: an empty concept_key is a bad row (it is the key used by the importer in T5).
    [Fact]
    public void Parse_EmptyConceptKey_ThrowsWithLineNumber()
    {
    }

    // Proposal: rank must be positive (1 = most common), so 0 and negative numbers are errors.
    [Fact]
    public void Parse_ZeroOrNegativeRank_ThrowsWithLineNumber()
    {
    }
}
