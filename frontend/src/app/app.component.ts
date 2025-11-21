import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'ai-root',
  standalone: true,
  imports: [RouterOutlet, MatToolbarModule],
  template: `
    <mat-toolbar color="primary" class="app-toolbar">
      <span>AI Code Generation Playground</span>
    </mat-toolbar>
    <main class="app-shell">
      <section class="app-container">
        <router-outlet />
      </section>
    </main>
  `,
  styles: [
    `
      .app-toolbar {
        position: sticky;
        top: 0;
        z-index: 3;
        box-shadow: 0 10px 30px -20px rgba(15, 23, 42, 0.6);
      }
      .app-shell {
        min-height: calc(100vh - 64px);
        padding: 2.5rem 1.5rem 3.5rem;
        display: flex;
        justify-content: center;
      }
      .app-container {
        width: min(1200px, 100%);
        display: block;
      }
    `
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent {}
