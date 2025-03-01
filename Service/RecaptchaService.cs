using System.Text.Json;
using System.Text.Json.Serialization;

public class ReСaptchaService
{
    private readonly string _secretKey;
    private readonly HttpClient _httpClient;

    public ReСaptchaService(IConfiguration configuration)
    {
        _secretKey = configuration["ReCaptcha:SecretKey"];
        _httpClient = new HttpClient();
    }

    public async Task<bool> Validate(string recaptcha)
    {
        if (string.IsNullOrEmpty(recaptcha))
        {
            return false;
        }

        var response = await _httpClient.GetStringAsync(
            $"https://www.google.com/recaptcha/api/siteverify?secret={_secretKey}&response={recaptcha}"
        );

        var json = JsonSerializer.Deserialize<RecaptchaVerificationResponse>(response);
        return json?.Success ?? false;
    }
}

public class RecaptchaVerificationResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
}