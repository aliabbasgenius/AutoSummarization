# AutoSummarization

## AI Code Generation Playground

This solution demonstrates an end-to-end playground to experiment with AI-assisted code generation using:

- **Angular (standalone, signals, Angular Material, Vite)** for the UI
- **ASP.NET Core 9 minimal API** for orchestration and OpenAI integration
- **OpenAI Chat Completions** with two contrasting strategies: full-context code prompting vs. auto-summarization prompting

## Project layout

```
frontend/  Angular 21-ready client app built on Vite
backend/   .NET 9 solution with Domain, Application, Infrastructure, and API layers
```

## Prerequisites

- Node.js 18+ and pnpm/npm (pnpm recommended)
- .NET SDK 9 preview (or newer) – required for `net9.0`
- OpenAI API key stored securely (environment variables, Azure Key Vault, or `dotnet user-secrets`)

## Getting started

### 1. Configure secrets

```
cd backend/src/AutoSummarization.Api
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "<your-api-key>"
```

Optional overrides:

```
dotnet user-secrets set "OpenAI:BaseUrl" "https://api.openai.com/"
dotnet user-secrets set "OpenAI:DefaultModel" "gpt-4.1-mini"
```

### 2. Install dependencies

```powershell
# Frontend
cd ../../..
cd frontend
npm install

# Backend
cd ..\backend
dotnet restore
```

### 3. Run the solution

```powershell
# In one terminal
cd frontend
npm run dev

# In another terminal
cd backend/src/AutoSummarization.Api
dotnet run
```

Browse to `http://localhost:4200` for the UI and `https://localhost:5001/swagger` for API docs.

## Frontend overview

- Feature module at `src/app/modules/ai` handles AI configuration, services, and the comparison UI.
- `CompareResultsComponent` surfaces two text areas (source and generated code), a strategy selector, and metrics (token counts, response time).
- Services:
  - `AiConfigService` exposes runtime OpenAI configuration.
  - `FullcontextCodegenService` calls the backend with full source context.
  - `SummarizationService` sends summarized context requests.
  - `AzureApiService` is a thin wrapper for backend calls and future Azure API Management support.
- Environment files (`environment.ts`) never embed secrets; keys must be injected via runtime configuration.

## Backend overview

- **Domain** layer stores `OrderBlueprint` and other core models.
- **Application** layer orchestrates strategies, validation (FluentValidation), and mapping (AutoMapper).
- **Infrastructure** wraps the OpenAI REST API using `HttpClient` and typed options.
- **API** layer hosts minimal API endpoints, Serilog logging, Swagger, and health checks.
- Endpoint `POST /api/codegen/order` accepts `OrderGenerationRequestDto` and returns `CodeGenerationResponseDto` with token usage metrics.

## Extensibility

- Strategies implement `ICodeGenerationStrategy`; register new strategies to experiment with RAG or fine-tuning.
- Swap OpenAI provider by implementing `IOpenAiClient` (e.g., Azure OpenAI, Anthropic) without touching application logic.
- Introduce persistence or telemetry in infrastructure without leaking concerns into upper layers.

## Testing (next milestones)

- Add unit tests for orchestrator/strategies (xUnit + FluentAssertions).
- Integration tests for REST endpoints via `WebApplicationFactory`.
- Cypress/Playwright or Angular component tests for the comparison UI.

## Security notes

- Never commit real secrets. Rely on environment variables or `dotnet user-secrets` during local development.
- Update CORS and proxy settings before exposing publicly.
- Consider Azure API Management, rate limiting, and request auditing for production.
