using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace WordWise.Api.Infrastructure.OpenApi;

// Copies the values of [AllowedValues] into the OpenAPI `enum` of a schema. The generator doesn't do this
// on its own (documented: "[AllowedValues] does not set the enum values of a property"), so without it
// ?base= would be a plain string in the contract and in the generated Angular client.
// It only describes the rule; the validation itself is still done by [AllowedValues] at runtime.
// A schema transformer runs once for every schema the generator creates (parameters, bodies, properties).
// Docs: https://learn.microsoft.com/aspnet/core/fundamentals/openapi/customize-openapi#use-schema-transformers
//   https://learn.microsoft.com/aspnet/core/fundamentals/openapi/include-metadata#enum
internal sealed class AllowedValuesSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (FindAllowedValues(context) is { } allowedValues)
        {
            // OpenAPI `enum` holds JSON values, so each C# value is serialized ("en" → JSON string "en").
            schema.Enum = [.. allowedValues.Values.Select(value => JsonSerializer.SerializeToNode(value)!)];
        }

        return Task.CompletedTask;
    }

    private static AllowedValuesAttribute? FindAllowedValues(OpenApiSchemaTransformerContext context)
    {
        // 1) A property of a JSON body (request or response DTO): System.Text.Json knows the C# property.
        if (context.JsonPropertyInfo?.AttributeProvider is { } attributeProvider)
        {
            return attributeProvider
                .GetCustomAttributes(typeof(AllowedValuesAttribute), inherit: true)
                .OfType<AllowedValuesAttribute>()
                .FirstOrDefault();
        }

        // 2) A query parameter bound from a property of a complex type, e.g. WordCardsRequest.BaseLanguage.
        //    MVC's model metadata knows the type (ContainerType) and the property the parameter came from.
        //    Docs: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.openapi.openapischematransformercontext.parameterdescription
        if (context.ParameterDescription?.ModelMetadata is { ContainerType: { } containerType, PropertyName: { } propertyName })
        {
            return containerType.GetProperty(propertyName)?.GetCustomAttribute<AllowedValuesAttribute>();
        }

        return null;
    }
}
