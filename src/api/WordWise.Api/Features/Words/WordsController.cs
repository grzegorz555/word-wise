using Microsoft.AspNetCore.Mvc;

namespace WordWise.Api.Features.Words;

// Thin controller (ADR-0002): HTTP in, validation by [ApiController], logic in WordCardQueries.
// Docs: https://learn.microsoft.com/aspnet/core/web-api/
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")] // only JSON in OpenAPI (default lists text/plain and text/json as well)
public sealed class WordsController(WordCardQueries queries) : ControllerBase
{
    // GET /api/words/cards?base=pl&target=tr&order=sequential
    // [FromQuery] on a complex type binds each of its properties from the query string (spec 001, Contract).
    // Docs: https://learn.microsoft.com/aspnet/core/mvc/models/model-binding
    // [EndpointName] = the OpenAPI operationId, so the generated Angular function is getWordCards
    // (without it the generator guesses apiWordsCardsGet).
    // Docs: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata#include-openapi-metadata-for-endpoints
    [HttpGet("cards")]
    [EndpointName("GetWordCards")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
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
