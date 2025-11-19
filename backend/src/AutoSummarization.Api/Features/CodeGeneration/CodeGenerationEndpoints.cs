using AutoSummarization.Application.Features.CodeGeneration.DTOs;
using AutoSummarization.Application.Features.CodeGeneration.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace AutoSummarization.Api.Features.CodeGeneration;

public static class CodeGenerationEndpoints
{
    public static IEndpointRouteBuilder MapCodeGenerationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/codegen")
            .WithTags("AI Code Generation");

        group.MapPost("/order", async (
            OrderGenerationRequestDto request,
            ICodeGenerationOrchestrator orchestrator,
            IValidator<OrderGenerationRequestDto> validator,
            CancellationToken cancellationToken
        ) =>
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var response = await orchestrator.HandleAsync(request, cancellationToken);
            return Results.Ok(response);
        })
        .Produces<CodeGenerationResponseDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithName("ExecuteCodeGeneration");

        return endpoints;
    }
}
