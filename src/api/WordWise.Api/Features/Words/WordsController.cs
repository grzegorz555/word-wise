using Microsoft.AspNetCore.Mvc;

namespace WordWise.Api.Features.Words;

// Thin controller (ADR-0002): HTTP in, validation by [ApiController], logic in WordCardQueries.
// Docs: https://learn.microsoft.com/aspnet/core/web-api/
[ApiController]
[Route("api/[controller]")]
public sealed class WordsController(WordCardQueries queries) : ControllerBase
{
    // GET /api/words/cards?base=pl&target=tr&order=sequential
    // [FromQuery] on a complex type binds each of its properties from the query string (spec 001, Contract).
    // Docs: https://learn.microsoft.com/aspnet/core/mvc/models/model-binding
    // [EndpointName] = the OpenAPI operationId, so the generated Angular function is getWordCards
    // (without it the generator guesses apiWordsCardsGet).
    // [ProducesResponseType] with a content type per status code: JSON for 200, problem+json for 400.
    // Not [Produces("application/json")]: that is also a result filter, so the automatic 400 would be sent
    // as application/json instead of application/problem+json (RFC 9457).
    // Docs: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata#include-openapi-metadata-for-endpoints
    [HttpGet("cards")]
    [EndpointName("GetWordCards")]
    [ProducesResponseType<IReadOnlyList<WordCardResponse>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<ActionResult<IReadOnlyList<WordCardResponse>>> GetCards(
        [FromQuery] WordCardsQuery query,
        CancellationToken cancellationToken)
    {
        var cards = await queries.GetCardsAsync(query, cancellationToken);

        // Ok(...) is needed: ActionResult<T> has no implicit conversion from an interface type (IReadOnlyList).
        // Docs: https://learn.microsoft.com/aspnet/core/web-api/action-return-types#actionresultt-type
        return Ok(cards);
    }
}
