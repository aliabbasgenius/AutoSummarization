using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoSummarization.Application.Features.CodeGeneration.DTOs;
using AutoSummarization.Application.Features.CodeGeneration.Validators;
using AutoSummarization.Domain.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AutoSummarization.Application.Features.CodeGeneration.Services;

public sealed class CodeGenerationOrchestrator(
    IEnumerable<ICodeGenerationStrategy> strategies,
    IValidator<OrderGenerationRequestDto> validator,
    IMapper mapper,
    ILogger<CodeGenerationOrchestrator> logger
) : ICodeGenerationOrchestrator
{
    private readonly IReadOnlyDictionary<string, ICodeGenerationStrategy> _strategies =
        strategies.ToDictionary(strategy => strategy.Name, StringComparer.OrdinalIgnoreCase);

    public async Task<CodeGenerationResponseDto> HandleAsync(OrderGenerationRequestDto request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        if (!_strategies.TryGetValue(request.Mode, out var strategy))
        {
            throw new ValidationException($"No strategy registered for mode '{request.Mode}'.");
        }

        var blueprint = mapper.Map<OrderBlueprint>(request);

        logger.LogInformation(
            "Executing {Strategy} strategy for feature {Feature} using model {Model}.",
            strategy.Name,
            request.Feature,
            request.Model ?? "default"
        );

        var response = await strategy.ExecuteAsync(blueprint, request, cancellationToken);
        return response;
    }
}
