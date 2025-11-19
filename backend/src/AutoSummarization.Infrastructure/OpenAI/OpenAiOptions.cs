using System.ComponentModel.DataAnnotations;

namespace AutoSummarization.Infrastructure.OpenAI;

public sealed class OpenAiOptions
{
    public const string SectionName = "OpenAI";

    [Required]
    public string ApiKey { get; init; } = string.Empty;

    public string? BaseUrl { get; init; }

    public string DefaultModel { get; init; } = "gpt-4.1-mini";
}
