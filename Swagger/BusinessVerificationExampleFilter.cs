using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EvalRightAPI.Swagger;

/// <summary>
/// Adds the sample request body example to the Business Verification POST endpoint in Swagger.
/// </summary>
public class BusinessVerificationExampleFilter : IOperationFilter
{
    /// <summary>
    /// Sample per Equifax "Business Verification Solution (KYB) APIs Access Instructions v1.3":
    /// flat businessSubject (businessName, address, identityNumberType, identityNumber), Principal, Preferences (PascalCase).
    /// </summary>
    private static readonly string SampleJson = """
        {
          "businessSubject": {
            "businessName": "ABC Logistics LLC",
            "address": {
              "addressLine1": "123 Market St",
              "city": "Dallas",
              "state": "TX",
              "postalCode": "75201",
              "countryCode": "840"
            },
            "identityNumberType": "EIN",
            "identityNumber": "123456789"
          },
          "Preferences": {
            "includeTINValidation": "Y",
            "includeBusinessVerification": "Y"
          }
        }
        """;

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.RelativePath?.Contains("business-verification") != true || context.ApiDescription.HttpMethod != "POST")
            return;

        operation.RequestBody ??= new OpenApiRequestBody { Required = true };
        operation.RequestBody.Content ??= new Dictionary<string, OpenApiMediaType>();
        if (!operation.RequestBody.Content.ContainsKey("application/json"))
            operation.RequestBody.Content["application/json"] = new OpenApiMediaType();
        operation.RequestBody.Content["application/json"].Example = new OpenApiString(SampleJson);
    }
}
