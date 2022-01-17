import { Routes } from '@angular/router';

import { FullComponent } from './layouts/full/full.component';
import { LoginComponent } from './login/login.component';
import { MsalGuard } from '@azure/msal-angular';

export const Approutes: Routes = [
  {
    path: '',
    component: FullComponent,
    canActivate: [MsalGuard],
    children: [
      { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
      },
      {
        path: 'administration',
        loadChildren: () =>
          import('./administration/administration.module').then(
            (m) => m.AdministrationModule
          ),
      },
    ],
  },
  {
    path: '**',
    redirectTo: '/dashboard',
  },
  {
    path: 'login',
    component: LoginComponent,
  },
];
