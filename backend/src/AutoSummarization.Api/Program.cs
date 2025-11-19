using AutoSummarization.Api.Configuration;
using AutoSummarization.Api.Features.CodeGeneration;
using AutoSummarization.Infrastructure.OpenAI;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .WriteTo.Console();
});

builder.Services.AddOptions<OpenAiOptions>()
    .BindConfiguration(OpenAiOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplicationServices()
    .AddFluentValidationAutoValidation();

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
    options.AddPolicy("frontend", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    )
);

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler();
}

app.UseHttpsRedirection();
app.UseCors("frontend");
app.MapHealthChecks("/health");
app.MapCodeGenerationEndpoints();

await app.RunAsync();
