namespace AutoSummarization.Application.Features.CodeGeneration.DTOs;

public sealed record TokenUsageDto(int PromptTokens, int CompletionTokens)
{
    public int TotalTokens => PromptTokens + CompletionTokens;
}
