import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { AiConfigService } from './ai-config.service';

interface AzureRequest<TPayload> {
  path: string;
  payload: TPayload;
}

@Injectable({ providedIn: 'root' })
export class AzureApiService {
  constructor(
    private readonly http: HttpClient,
    private readonly config: AiConfigService
  ) {}

  async post<TPayload, TResponse>(request: AzureRequest<TPayload>): Promise<TResponse> {
    const url = `${this.config.apiBaseUrl()}${request.path}`;
    const response = await firstValueFrom(this.http.post<TResponse>(url, request.payload));
    return response;
  }
}
