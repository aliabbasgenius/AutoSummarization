import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { AiConfigService } from './ai-config.service';
import { CodegenMode } from '../models/codegen-mode.enum';
import { CodegenRequest } from '../models/codegen-request.model';
import { CodegenResult } from '../models/codegen-result.model';

@Injectable({ providedIn: 'root' })
export class SummarizationService {
  constructor(
    private readonly http: HttpClient,
    private readonly config: AiConfigService
  ) {}

  async summarizeAndGenerate(request: CodegenRequest): Promise<CodegenResult> {
    const payload = {
      mode: CodegenMode.AutoSummarization,
      source: request.source,
      feature: request.feature,
      language: request.language,
      model: this.config.model()
    };

    const response = await firstValueFrom(
      this.http.post<CodegenResult>(`${this.config.apiBaseUrl()}/codegen/order`, payload)
    );

    return response;
  }
}
