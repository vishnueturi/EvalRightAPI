using System.Text.Json.Serialization;

namespace EvalRightAPI.Models;

/// <summary>
/// Request body for Equifax Business Verification (KYB) API.
/// Structure per Equifax "Business Verification Solution (KYB) APIs Access Instructions v1.3" (04-24-2025).
/// </summary>
public class BusinessVerificationRequest
{
    [JsonPropertyName("businessSubject")]
    public BusinessSubject? BusinessSubject { get; set; }

    [JsonPropertyName("principals")]
    public Principal[]? Principals { get; set; }

    [JsonPropertyName("additionalFields")]
    public AdditionalField[]? AdditionalFields { get; set; }

    [JsonPropertyName("permissiblePurpose")]
    public PermissiblePurpose? PermissiblePurpose { get; set; }

    [JsonPropertyName("preferences")]
    public Preferences? Preferences { get; set; }
}

/// <summary>
/// Business subject with nested structures per Equifax doc.
/// </summary>
public class BusinessSubject
{
    [JsonPropertyName("name")]
    public BusinessName? Name { get; set; }

    [JsonPropertyName("businessName")]
    public string? BusinessName { get; set; }

    [JsonPropertyName("address")]
    public BusinessAddress? Address { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("identities")]
    public Identity[]? Identities { get; set; }
}

public class BusinessName
{
    [JsonPropertyName("businessName")]
    public string? Name { get; set; }
}

public class Identity
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }  // e.g. "EIN"

    [JsonPropertyName("identityNumber")]
    public string? IdentityNumber { get; set; }
}

public class BusinessAddress
{
    [JsonPropertyName("addressLine1")]
    public string? AddressLine1 { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("extendedZip")]
    public string? ExtendedZip { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }
}

public class Principal
{
    [JsonPropertyName("personName")]
    public PersonName? PersonName { get; set; }

    [JsonPropertyName("addresses")]
    public BusinessAddress[]? Addresses { get; set; }

    [JsonPropertyName("phones")]
    public Phone[]? Phones { get; set; }

    [JsonPropertyName("emails")]
    public Email[]? Emails { get; set; }

    [JsonPropertyName("identities")]
    public Identity[]? Identities { get; set; }

    [JsonPropertyName("dateOfBirth")]
    public DateOfBirth? DateOfBirth { get; set; }
}

public class PersonName
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("middleName")]
    public string? MiddleName { get; set; }
}

public class Phone
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }
}

public class Email
{
    [JsonPropertyName("emailId")]
    public string? EmailId { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class DateOfBirth
{
    [JsonPropertyName("date")]
    public string? Date { get; set; }
}

public class AdditionalField
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

public class PermissiblePurpose
{
    [JsonPropertyName("pin")]
    public string? Pin { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

public class Preferences
{
    [JsonPropertyName("includeTINValidation")]
    public string? IncludeTINValidation { get; set; }  // "Y" or "N"

    [JsonPropertyName("includeBusinessVerification")]
    public string? IncludeBusinessVerification { get; set; }  // "Y" or "N"
}
