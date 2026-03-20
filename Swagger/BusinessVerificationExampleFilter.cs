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
    /// Sample request body for the Business Verification endpoint.
    /// </summary>
    private static readonly string SampleJson = """
    {
      "businessSubject": {
        "name": {
          "businessName": "LABERNZd"
        },
        "address": {
          "addressLine1": "251002 OCEANSPRAY CT 251002",
          "city": "CHESHIRE",
          "state": "CT",
          "postalCode": "06410-1212",
          "extendedZip": "",
          "countryCode": "840"
        },
        "phoneNumber": "1111111181",
        "identities": [
          {
            "type": "",
            "identityNumber": "668548724"
          }
        ]
      },
      "principals": [
        {
          "personName": {
            "firstName": "Melane",
            "lastName": "McolenUAT",
            "middleName": ""
          },
          "addresses": [
            {
              "addressLine1": "251002 OCEANSPRAY CT 251002",
              "city": "CHESHIRE",
              "state": "CT",
              "postalCode": "06410-1212",
              "extendedZip": "",
              "countryCode": "840"
            }
          ],
          "phones": [
            {
              "type": "mobile",
              "number": "1111111156",
              "countryCode": "1"
            }
          ],
          "emails": [
            {
              "emailId": "support@equifax.com",
              "type": "personal"
            }
          ],
          "identities": [
            {
              "type": "SSN",
              "identityNumber": "799005091"
            }
          ],
          "dateOfBirth": {
            "date": "12241995"
          }
        }
      ],
      "additionalFields": [
        {
          "key": "CONSENT_OPTIN_URL",
          "value": "https://example.com/license"
        }
      ],
      "permissiblePurpose": {
        "pin": "",
        "value": ""
      },
      "preferences": {
        "includeTINValidation": "N",
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
