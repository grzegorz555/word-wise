using Microsoft.AspNetCore.Mvc;

namespace WordWise.Api.Tests.TestSupport;

// Test-only endpoint that always throws, so tests can check what a client sees for an unhandled exception.
// It lives in the test assembly, so the real API never has it: a test adds it as an MVC application part.
// Docs: https://learn.microsoft.com/aspnet/core/mvc/advanced/app-parts
[ApiController]
[Route(Path)]
public sealed class ThrowingTestController : ControllerBase
{
    public const string Path = "test/throw";
    public const string ExceptionMessage = "Internal detail that must not reach the client";

    // CA1822 asks for `static` when a method uses no instance data, but a static method is not an MVC action.
    // The rule's other fix is to use instance data, so the message includes Request.Path, as real messages often
    // include request details. https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1822
    [HttpGet]
    public IActionResult Get() => throw new InvalidOperationException($"{ExceptionMessage}: {Request.Path}");
}
