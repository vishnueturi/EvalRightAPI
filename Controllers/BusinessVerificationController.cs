using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace EvalRightAPI.Controllers
{
    /// <summary>
    /// Proxies Business Verification (KYB) requests to Equifax sandbox.
    /// Request format per Equifax "Business Verification Solution (KYB) APIs Access Instructions v1.3".
    /// </summary>
    [ApiController]    
    [Route("business/verification/v1/business-verification")]
    public class BusinessVerificationController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BusinessVerificationController> _logger;

        public BusinessVerificationController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<BusinessVerificationController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Submit a business verification (KYB) request to Equifax.
        /// Forwards request body to Equifax (optionally normalized when Equifax:NormalizeRequest is true).
        /// Matches Node.js wrapper: Bearer token, optional InquiryMemberNumber, same status/body returned.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Post()
        {
            try
            {
                using var reader = new StreamReader(Request.Body, Encoding.UTF8);
                var rawBody = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(rawBody))
                    return BadRequest("Request body is required.");

                // Validate and normalize JSON so we always forward valid JSON to Equifax.
                // This mirrors the Node.js wrapper behavior (JSON.stringify(req.body)).
                string bodyToSend;
                JsonElement incomingJson;
                try
                {
                    using var doc = JsonDocument.Parse(rawBody);
                    incomingJson = doc.RootElement.Clone();
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Invalid JSON in request body");
                    return BadRequest(new { error = "Invalid JSON", detail = ex.Message });
                }
                                
                // Re-serialize JSON to ensure consistent formatting and remove any stray comments/trailing commas
                bodyToSend = JsonSerializer.Serialize(incomingJson);
                
                var accessToken = await GetEquifaxAccessTokenAsync();
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    _logger.LogWarning("Unable to acquire Equifax access token.");
                    return StatusCode(500, new { error = "Server misconfiguration", message = "Equifax access token is not available" });
                }

                var equifaxUrl = _configuration["Equifax:BusinessVerificationUrl"]
                    ?? "https://api.sandbox.equifax.com/business/verification/v1/business-verification";

                _logger.LogInformation(
                    "Forwarding to Equifax: URL={Url}, BodyLength={BodyLength}",
                    equifaxUrl, bodyToSend.Length);

                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, equifaxUrl)
                {
                    Content = new StringContent(bodyToSend, Encoding.UTF8, "application/json")
                };

                // Ensure Content-Type matches what the Equifax sandbox expects (no charset)
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var inquiryMemberNumber = _configuration["Equifax:InquiryMemberNumber"];
                //if (!string.IsNullOrWhiteSpace(inquiryMemberNumber))
                //    request.Headers.TryAddWithoutValidation("InquiryMemberNumber", inquiryMemberNumber);

                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Equifax response status: {StatusCode}", response.StatusCode);

                if (!response.IsSuccessStatusCode)
                    _logger.LogWarning("Equifax returned {StatusCode}: {Content}", response.StatusCode, content);

                return new ContentResult
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Business verification request failed");
                return StatusCode(500, new { error = "Upstream error", message = ex.Message });
            }
        }

        private async Task<string?> GetEquifaxAccessTokenAsync()
        {
            var tokenUrl = _configuration["Equifax:TokenUrl"] ?? "https://api.sandbox.equifax.com/v2/oauth/token";
            var tokenScope = _configuration["Equifax:TokenScope"] ?? "https://api.equifax.com/business/verification/v1";
            var clientId = _configuration["Equifax:ClientId"];
            var clientSecret = _configuration["Equifax:ClientSecret"];

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {                
                _logger.LogWarning("Equifax client credentials are not configured (ClientId/ClientSecret)");
                return null;
            }

            try
            {
                using var client = _httpClientFactory.CreateClient();
                using var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);

                var formData = new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["scope"] = tokenScope
                };

                request.Content = new FormUrlEncodedContent(formData);

                var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Equifax token request failed ({StatusCode}): {Content}", response.StatusCode, content);                    
                    return null;
                }

                var token = JsonSerializer.Deserialize<EquifaxTokenResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (token == null || string.IsNullOrWhiteSpace(token.access_token))
                {
                    _logger.LogWarning("Equifax token response is invalid: {Content}", content);                    
                    return null;
                }

                return token.access_token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to request Equifax access token.");                
                return null;
            }
        }

        private sealed record EquifaxTokenResponse(
            string access_token,
            string token_type,
            int expires_in,
            string issued_at,
            string scope);
    }
}
