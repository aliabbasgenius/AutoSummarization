import { Injectable, computed, signal } from '@angular/core';
import { environment } from '@env/environment';

type OpenAiConfig = {
  apiBaseUrl: string;
  model: string;
};

@Injectable({ providedIn: 'root' })
export class AiConfigService {
  private readonly configState = signal<OpenAiConfig>({
    apiBaseUrl: environment.openAi.apiBaseUrl,
    model: environment.openAi.model
  });

  readonly apiBaseUrl = computed(() => this.configState().apiBaseUrl);
  readonly model = computed(() => this.configState().model);

  update(partial: Partial<OpenAiConfig>): void {
    this.configState.update(current => ({ ...current, ...partial }));
  }
}
