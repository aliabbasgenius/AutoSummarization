using System.Collections.Generic;

namespace AutoSummarization.Application.Features.CodeGeneration.DTOs;

public sealed record OrderGenerationRequestDto(
    string Mode,
    string Source,
    string Feature,
    string Language,
    string? Model,
    IDictionary<string, string>? Metadata
);
