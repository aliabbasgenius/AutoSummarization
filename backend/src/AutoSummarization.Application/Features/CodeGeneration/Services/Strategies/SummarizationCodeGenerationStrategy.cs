using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoSummarization.Application.Abstractions;
using AutoSummarization.Application.Features.CodeGeneration.DTOs;
using AutoSummarization.Domain.Models;

namespace AutoSummarization.Application.Features.CodeGeneration.Services.Strategies;

public sealed class SummarizationCodeGenerationStrategy(IOpenAiClient openAiClient) : ICodeGenerationStrategy
{
    public string Name => "auto-summarization";

    public async Task<CodeGenerationResponseDto> ExecuteAsync(
        OrderBlueprint blueprint,
        OrderGenerationRequestDto request,
        CancellationToken cancellationToken
    )
    {
        var systemPrompt = "You are an AI pair-programmer that summarizes legacy modules before generating target code.";

        var summaryPrompt = new StringBuilder();
        summaryPrompt.AppendLine("Summarize the following module focusing on domain constraints, dependencies, and public APIs.");
        summaryPrompt.AppendLine("Return bullet points only.");
        summaryPrompt.AppendLine();
        summaryPrompt.AppendLine("```source");
        summaryPrompt.AppendLine(blueprint.SourceCode);
        summaryPrompt.AppendLine("```");

        var summarizeResult = await openAiClient.GenerateAsync(
            request.Model ?? "gpt-4.1-mini",
            systemPrompt,
            summaryPrompt.ToString(),
            cancellationToken
        );

        var generationPrompt = new StringBuilder();
        generationPrompt.AppendLine("Based on the summary below, implement OrderController using ASP.NET Core 9 minimal API style.");
        generationPrompt.AppendLine("- Include separated DTOs, FluentValidation, AutoMapper profiles, and dependency injection wiring.");
        generationPrompt.AppendLine("- Emit fully compilable C# 13 code.");
        generationPrompt.AppendLine();
        generationPrompt.AppendLine("Summary:");
        generationPrompt.AppendLine(summarizeResult.GeneratedCode);

        var stopwatch = Stopwatch.StartNew();
        var generationResult = await openAiClient.GenerateAsync(
            request.Model ?? "gpt-4.1-mini",
            systemPrompt,
            generationPrompt.ToString(),
            cancellationToken
        );
        stopwatch.Stop();

        var totalPromptTokens = summarizeResult.Usage.PromptTokens + generationResult.Usage.PromptTokens;
        var totalCompletionTokens = summarizeResult.Usage.CompletionTokens + generationResult.Usage.CompletionTokens;

        return new CodeGenerationResponseDto(
            generationResult.GeneratedCode,
            generationResult.Summary,
            Name,
            totalPromptTokens,
            totalCompletionTokens,
            (long)stopwatch.Elapsed.TotalMilliseconds
        );
    }
}
