using AutoSummarization.Application.Abstractions;
using AutoSummarization.Application.Features.CodeGeneration.Profiles;
using AutoSummarization.Application.Features.CodeGeneration.Services;
using AutoSummarization.Application.Features.CodeGeneration.Services.Strategies;
using AutoSummarization.Application.Features.CodeGeneration.Validators;
using AutoSummarization.Infrastructure.OpenAI;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AutoSummarization.Api.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CodeGenerationProfile).Assembly);
        services.AddValidatorsFromAssemblyContaining<OrderGenerationRequestValidator>();

        services.AddScoped<ICodeGenerationOrchestrator, CodeGenerationOrchestrator>();
        services.AddScoped<ICodeGenerationStrategy, FullContextCodeGenerationStrategy>();
        services.AddScoped<ICodeGenerationStrategy, SummarizationCodeGenerationStrategy>();

        services.AddHttpClient("openai");
        services.AddSingleton<IOpenAiClient, OpenAiClient>();

        return services;
    }
}
