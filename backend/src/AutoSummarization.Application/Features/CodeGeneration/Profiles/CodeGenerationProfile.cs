using AutoMapper;
using AutoSummarization.Application.Features.CodeGeneration.DTOs;
using AutoSummarization.Domain.Models;

namespace AutoSummarization.Application.Features.CodeGeneration.Profiles;

public sealed class CodeGenerationProfile : Profile
{
    public CodeGenerationProfile()
    {
        CreateMap<OrderGenerationRequestDto, OrderBlueprint>()
            .ForCtorParam(nameof(OrderBlueprint.FeatureName), x => x.MapFrom(src => src.Feature))
            .ForCtorParam(nameof(OrderBlueprint.SourceCode), x => x.MapFrom(src => src.Source))
            .ForCtorParam(nameof(OrderBlueprint.Language), x => x.MapFrom(src => src.Language));
    }
}
