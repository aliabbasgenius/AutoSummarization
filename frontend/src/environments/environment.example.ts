export const environment = {
  production: false,
  openAi: {
    apiBaseUrl: 'https://localhost:5001/api',
    model: 'gpt-4.1-mini',
    // The API key is resolved at runtime. Inject via environment variables or Azure Key Vault.
    apiKey: 'USE_ENVIRONMENT_VARIABLES'
  }
};
