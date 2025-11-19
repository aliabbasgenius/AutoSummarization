import { CodegenMode } from './codegen-mode.enum';

export interface CodegenResult {
  mode: CodegenMode;
  generatedCode: string;
  summary?: string;
  elapsedMilliseconds?: number;
  promptTokens?: number;
  completionTokens?: number;
}
