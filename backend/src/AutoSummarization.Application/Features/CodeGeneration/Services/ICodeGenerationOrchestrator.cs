using System.Threading;
using System.Threading.Tasks;
using AutoSummarization.Application.Features.CodeGeneration.DTOs;

namespace AutoSummarization.Application.Features.CodeGeneration.Services;

public interface ICodeGenerationOrchestrator
{
    Task<CodeGenerationResponseDto> HandleAsync(OrderGenerationRequestDto request, CancellationToken cancellationToken);
}
