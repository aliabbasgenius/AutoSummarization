using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoSummarization.Application.Abstractions;
using AutoSummarization.Application.Features.CodeGeneration.DTOs;
using AutoSummarization.Domain.Models;

namespace AutoSummarization.Application.Features.CodeGeneration.Services.Strategies;

public sealed class FullContextCodeGenerationStrategy(IOpenAiClient openAiClient) : ICodeGenerationStrategy
{
    public string Name => "full-context";

    public async Task<CodeGenerationResponseDto> ExecuteAsync(
        OrderBlueprint blueprint,
        OrderGenerationRequestDto request,
        CancellationToken cancellationToken
    )
    {
        var systemPrompt = "You are an expert ASP.NET Core architect who writes production-ready APIs.";

        var userPromptBuilder = new StringBuilder();
        userPromptBuilder.AppendLine("Generate OrderController using C# (ASP.NET Core 9) with CRUD, DI, DTOs, validation, async/await.");
        userPromptBuilder.AppendLine("Respond with fully compilable C# code only.");
        userPromptBuilder.AppendLine();
        userPromptBuilder.AppendLine("Existing source context:");
        userPromptBuilder.AppendLine("```csharp");
        userPromptBuilder.AppendLine(blueprint.SourceCode);
        userPromptBuilder.AppendLine("```");

        var stopwatch = Stopwatch.StartNew();
        var generationResult = await openAiClient.GenerateAsync(
            request.Model ?? "gpt-4.1-mini",
            systemPrompt,
            userPromptBuilder.ToString(),
            cancellationToken
        );
        stopwatch.Stop();

        return new CodeGenerationResponseDto(
            generationResult.GeneratedCode,
            generationResult.Summary,
            Name,
            generationResult.Usage.PromptTokens,
            generationResult.Usage.CompletionTokens,
            (long)stopwatch.Elapsed.TotalMilliseconds
        );
    }
}
