import { Component, AfterViewInit } from '@angular/core';
@Component({
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements AfterViewInit {
  subtitle1: string;
  subtitle2: string;
  subtitle3: string;
  constructor() {
    this.subtitle1 = 'Tenants Widget';
    this.subtitle2 = 'User Accounts Widget';
    this.subtitle3 = 'Roles Widget';

  }

  ngAfterViewInit() {}
}
