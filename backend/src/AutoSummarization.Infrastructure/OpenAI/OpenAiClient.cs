using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoSummarization.Application.Abstractions;
using AutoSummarization.Application.Features.CodeGeneration.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AutoSummarization.Infrastructure.OpenAI;

public sealed class OpenAiClient : IOpenAiClient
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenAiOptions _options;
    private readonly ILogger<OpenAiClient> _logger;

    public OpenAiClient(IHttpClientFactory httpClientFactory, IOptions<OpenAiOptions> options, ILogger<OpenAiClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<(string GeneratedCode, string? Summary, TokenUsageDto Usage)> GenerateAsync(
        string model,
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            _logger.LogWarning("OpenAI API key is missing. Returning placeholder response.");
            return (
                "// OpenAI API key missing. Configure user secrets: dotnet user-secrets set OpenAI:ApiKey <value>",
                "No API call executed.",
                new TokenUsageDto(0, 0)
            );
        }

        var client = _httpClientFactory.CreateClient("openai");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

        if (!string.IsNullOrWhiteSpace(_options.BaseUrl))
        {
            client.BaseAddress = new Uri(_options.BaseUrl, UriKind.Absolute);
        }

        var payload = new
        {
            model = string.IsNullOrWhiteSpace(model) ? _options.DefaultModel : model,
            messages = new object[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            }
        };

        using var response = await client.PostAsJsonAsync("v1/chat/completions", payload, SerializerOptions, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError(
                "OpenAI request failed with status {StatusCode}: {Problem}",
                (int)response.StatusCode,
                problem
            );

            return (
                $"// OpenAI request failed with status {(int)response.StatusCode}. Inspect server logs for details.",
                problem,
                new TokenUsageDto(0, 0)
            );
        }

        var document = await response.Content.ReadFromJsonAsync<JsonElement>(SerializerOptions, cancellationToken);
        var content = document.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;

        int promptTokens = 0;
        int completionTokens = 0;

        if (document.TryGetProperty("usage", out var usageElement))
        {
            promptTokens = usageElement.GetProperty("prompt_tokens").GetInt32();
            completionTokens = usageElement.GetProperty("completion_tokens").GetInt32();
        }

        return (
            content,
            "Generated via OpenAI Chat Completions.",
            new TokenUsageDto(promptTokens, completionTokens)
        );
    }
}
