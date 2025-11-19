import { AsyncPipe, DatePipe, JsonPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, effect, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';

import { CodegenMode } from '../models/codegen-mode.enum';
import { CodegenRequest } from '../models/codegen-request.model';
import { CodegenResult } from '../models/codegen-result.model';
import { FullContextCodegenService } from '../services/fullcontext-codegen.service';
import { SummarizationService } from '../services/summarization.service';

@Component({
  selector: 'ai-compare-results',
  standalone: true,
  imports: [
    AsyncPipe,
    DatePipe,
    JsonPipe,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatOptionModule,
    MatSelectModule
  ],
  templateUrl: './compare-results.component.html',
  styleUrls: ['./compare-results.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CompareResultsComponent {
  readonly CodegenMode = CodegenMode;

  readonly selectedModeControl = new FormControl<CodegenMode>(CodegenMode.FullContext, {
    nonNullable: true
  });

  readonly sourceCodeControl = new FormControl<string>(
    `// Paste the original module or feature to test
public class SampleModule { }
`,
    { nonNullable: true }
  );

  readonly featureControl = new FormControl<string>('OrderController', { nonNullable: true });

  readonly results = signal<CodegenResult[]>([]);
  readonly isLoading = signal(false);
  readonly lastRun = signal<Date | null>(null);
  readonly errorMessage = signal<string | null>(null);

  readonly requestPreview = computed<CodegenRequest>(() => ({
    mode: this.selectedModeControl.value,
    source: this.sourceCodeControl.value,
    feature: this.featureControl.value,
    language: 'csharp'
  }));

  readonly metrics = computed(() => {
    const [primary] = this.results();
    if (!primary) {
      return null;
    }

    return {
      elapsed: primary.elapsedMilliseconds,
      promptTokens: primary.promptTokens,
      completionTokens: primary.completionTokens
    };
  });

  constructor(
    private readonly fullContextService: FullContextCodegenService,
    private readonly summarizationService: SummarizationService
  ) {
    effect(() => {
      const error = this.errorMessage();
      if (error) {
        // eslint-disable-next-line no-console
        console.warn(error);
      }
    });
  }

  async runComparison(): Promise<void> {
    if (!this.sourceCodeControl.value) {
      this.errorMessage.set('Provide source code to generate from.');
      return;
    }

    this.errorMessage.set(null);
    this.isLoading.set(true);

    try {
      const request = this.requestPreview();
      const start = performance.now();

      const result =
        request.mode === CodegenMode.FullContext
          ? await this.fullContextService.generateWithFullContext(request)
          : await this.summarizationService.summarizeAndGenerate(request);

      const elapsed = Math.round(performance.now() - start);

      const normalized: CodegenResult = {
        ...result,
        mode: request.mode,
        elapsedMilliseconds: result.elapsedMilliseconds ?? elapsed,
        promptTokens: result.promptTokens ?? 0,
        completionTokens: result.completionTokens ?? 0
      };

      this.results.set([normalized, ...this.results().slice(0, 4)]);
      this.lastRun.set(new Date());
    } catch (error) {
      const message = error instanceof Error ? error.message : 'Unexpected error occurred.';
      this.errorMessage.set(message);
    } finally {
      this.isLoading.set(false);
    }
  }
}
