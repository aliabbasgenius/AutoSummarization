using AutoSummarization.Application.Features.CodeGeneration.DTOs;
using FluentValidation;

namespace AutoSummarization.Application.Features.CodeGeneration.Validators;

public sealed class OrderGenerationRequestValidator : AbstractValidator<OrderGenerationRequestDto>
{
    public OrderGenerationRequestValidator()
    {
        RuleFor(x => x.Mode)
            .NotEmpty()
            .Must(mode => mode is "full-context" or "auto-summarization")
            .WithMessage("Mode must be either 'full-context' or 'auto-summarization'.");

        RuleFor(x => x.Source)
            .NotEmpty()
            .MinimumLength(25)
            .WithMessage("Provide at least 25 characters of source code to summarize.");

        RuleFor(x => x.Feature)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.Language)
            .NotEmpty()
            .MaximumLength(32);
    }
}
