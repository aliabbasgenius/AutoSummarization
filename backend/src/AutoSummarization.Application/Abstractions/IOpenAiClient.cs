using System.Threading;
using System.Threading.Tasks;
using AutoSummarization.Application.Features.CodeGeneration.DTOs;

namespace AutoSummarization.Application.Abstractions;

public interface IOpenAiClient
{
    Task<(string GeneratedCode, string? Summary, TokenUsageDto Usage)> GenerateAsync(
        string model,
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken
    );
}
