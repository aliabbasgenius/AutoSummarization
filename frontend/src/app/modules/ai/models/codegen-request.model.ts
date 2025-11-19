import { CodegenMode } from './codegen-mode.enum';

export interface CodegenRequest {
  mode: CodegenMode;
  source: string;
  language: 'csharp' | 'typescript';
  feature: string;
}
