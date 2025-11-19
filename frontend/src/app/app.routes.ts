import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./modules/ai/compare-results/compare-results.component').then(m => m.CompareResultsComponent)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
