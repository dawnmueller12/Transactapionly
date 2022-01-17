import { Routes } from '@angular/router';
import { TenantsComponent } from './tenants/tenants.component';
import { UserAccountsComponent } from './useraccounts/useraccounts.component';
import { WidgetsComponent } from './widgets/widgets.component';
export const AdministrationRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'tenants',
        component: TenantsComponent,
        data: {
          title: 'Tenants',
          urls: [{ title: 'Tenant', url: '/tenants' }],
        },
      },
      {
        path: 'useraccounts',
        component: UserAccountsComponent,
        data: {
          title: 'UserAccounts',
          urls: [{ title: 'UserAccount', url: '/useraccounts' }],
        },
      },
      {
        path: 'widgets',
        component: WidgetsComponent,
        data: {
          title: 'Widgets',
          urls: [{ title: 'Widgets', url: '/widgets' }],
        },
      },
    ],
  },
];
