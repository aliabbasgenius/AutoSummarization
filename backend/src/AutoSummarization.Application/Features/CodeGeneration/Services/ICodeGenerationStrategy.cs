using System.Threading;
using System.Threading.Tasks;
using AutoSummarization.Application.Features.CodeGeneration.DTOs;
using AutoSummarization.Domain.Models;

namespace AutoSummarization.Application.Features.CodeGeneration.Services;

public interface ICodeGenerationStrategy
{
    string Name { get; }

    Task<CodeGenerationResponseDto> ExecuteAsync(OrderBlueprint blueprint, OrderGenerationRequestDto request, CancellationToken cancellationToken);
}
