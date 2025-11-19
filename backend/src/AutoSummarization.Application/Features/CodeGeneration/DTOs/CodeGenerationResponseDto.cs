namespace AutoSummarization.Application.Features.CodeGeneration.DTOs;

public sealed record CodeGenerationResponseDto(
    string GeneratedCode,
    string? Summary,
    string Strategy,
    int PromptTokens,
    int CompletionTokens,
    long ElapsedMilliseconds
);
