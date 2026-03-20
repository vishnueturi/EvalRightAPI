# EvalRight API - Business Verification (KYB)

A **ASP.NET Core 8** API that proxies Business Verification (Know Your Business) requests to the **Equifax sandbox** environment. Built for secure, type-safe integration with Equifax's KYB solution.

## Overview

- **Framework**: ASP.NET Core 8 (.NET 8)
- **Language**: C# 12
- **Purpose**: Secure proxy to Equifax Business Verification API
- **Features**:
  - Bearer token authentication with Equifax
  - Optional inquiry member number header
  - JSON request validation and normalization
  - Structured logging via ILogger
  - Swagger/OpenAPI documentation

## Prerequisites

- **.NET 8 SDK** ([Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
- **Visual Studio 2022** (or VS Code with C# extension) - optional
- **Equifax sandbox credentials**:
  - `ClientId`
  - `ClientSecret`

## Quick Start

### 1. Clone the Repository
```bash
git clone https://github.com/your-org/evalright-api.git
cd evalright-api
```

### 2. Configure Credentials
Copy the example configuration file and add your Equifax credentials:
```bash
cp appsettings.Development.json.example appsettings.Development.json
```

Edit `appsettings.Development.json` and add:
```json
{
  "Equifax": {
    "ClientId": "your-client-id-here",
    "ClientSecret": "your-client-secret-here"
  }
}
```

### 3. Run the API
```bash
dotnet run
```

The API will start at: `http://localhost:5000`

Swagger UI: `http://localhost:5000/swagger/index.html`

## Configuration

### appsettings.json
| Key | Description | Default |
|-----|-------------|---------|
| `Equifax:TokenUrl` | OAuth token endpoint | `https://api.sandbox.equifax.com/v2/oauth/token` |
| `Equifax:TokenScope` | OAuth scope | `https://api.equifax.com/business/verification/v1` |
| `Equifax:ClientId` | **Required** Equifax client ID | ❌ Must be set in Development config |
| `Equifax:ClientSecret` | **Required** Equifax client secret | ❌ Must be set in Development config |
| `Equifax:BusinessVerificationUrl` | KYB verification endpoint | `https://api.sandbox.equifax.com/business/verification/v1/business-verification` |
| `Equifax:InquiryMemberNumber` | Optional member number header | *(empty)* |

## API Endpoints

### POST /business/verification/v1/business-verification

Submit a business verification request to Equifax.

**Request Body**: 
- Content-Type: `application/json`
- Schema: See `Models/BusinessVerificationRequest.cs` and [Postman Collection](#testing)

**Response**: 
- Equifax response status and body are returned as-is
- Content-Type: `application/json`

**Example Request**:
```bash
curl -X POST http://localhost:5000/business/verification/v1/business-verification \
  -H "Content-Type: application/json" \
  -d @request-body.json
```

## Testing

### Using Postman
A Postman collection is included: `Business Verification (KYB)-Collection.json`

1. Import the collection into Postman
2. Set the `base_url` variable to `http://localhost:5000`
3. Execute requests against the running API

### Manual Testing
```bash
# Test with curl
curl -X POST http://localhost:5000/business/verification/v1/business-verification \
  -H "Content-Type: application/json" \
  -d '{
    "businessSubject": {
      "businessName": "Acme Corp",
      "address": {
        "addressLine1": "123 Main St",
        "city": "New York",
        "state": "NY",
        "postalCode": "10001",
        "countryCode": "US"
      }
    }
  }'
```

## Project Structure

```
EvalRightAPI/
├── Controllers/
│   └── BusinessVerificationController.cs  # Main API controller
├── Models/
│   └── BusinessVerificationRequest.cs     # Request/response models
├── Properties/
│   └── launchSettings.json                # Debug launch config
├── appsettings.json                       # Default config
├── appsettings.Development.json           # ⚠️ NOT committed (add to .gitignore)
├── Program.cs                             # ASP.NET Core startup
└── EvalRightAPI.csproj                    # Project file
```

## Development

### Building
```bash
dotnet build
```

### Running in Debug Mode
```bash
dotnet run
```

### Running Tests
```bash
dotnet test
```

## Security Considerations

⚠️ **Never commit sensitive data**:
- `appsettings.Development.json` (contains credentials)
- `.env` files
- `secrets.json`

**Best Practices**:
1. Use `appsettings.Development.json.example` as a template
2. Store production secrets in Azure Key Vault or similar
3. Use environment variables for deployment
4. Rotate Equifax credentials regularly

## Logging

The API uses structured logging with `ILogger<T>`. Logs include:
- Token request/response status
- JSON validation errors
- Equifax upstream errors
- Request forwarding details

Configure log levels in `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "EvalRightAPI": "Debug"
    }
  }
}
```

## Contributing

1. Create a feature branch: `git checkout -b feature/your-feature`
2. Commit changes: `git commit -am 'Add feature'`
3. Push to branch: `git push origin feature/your-feature`
4. Submit a Pull Request

## License

[Add your license here - e.g., MIT, Apache 2.0]

## Support

For issues with:
- **This API**: Open an issue on GitHub
- **Equifax Integration**: Contact Equifax support or consult their API documentation

## References

- [Equifax Business Verification API v1.3](https://www.equifax.com/)
- [ASP.NET Core 8 Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [System.Text.Json Documentation](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/)
