import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AdministrationRoutes } from './administration.routing';
import { TenantsComponent } from './tenants/tenants.component';
import { TenantsService } from './tenants/tenants.service';
import { WidgetsComponent } from './widgets/widgets.component';
import { WidgetsService } from './widgets/widgets.service';
import { UserAccountsComponent } from './useraccounts/useraccounts.component';
import { UserAccountsService } from './useraccounts/useraccounts.service';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdministrationRoutes),
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
  ],
  providers: [TenantsService, UserAccountsService, WidgetsService],
  declarations: [TenantsComponent, UserAccountsComponent, WidgetsComponent],
})
export class AdministrationModule {}
