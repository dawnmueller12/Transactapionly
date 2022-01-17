import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { protectedResources } from 'src/app/auth-config';
import { Tenant } from './tenant';

@Injectable()
export class TenantsService {
  constructor(private http: HttpClient) {}

  createTenant(tenant: Tenant) {
    return this.http.post(protectedResources.bohoTenantsApi.endpoint, tenant);
  }
  getTenants() {
    return this.http.get<{ IS_SUCCESS: boolean; RESPONSE: Tenant[] }>(
      protectedResources.bohoTenantsApi.endpoint
    );
  }
  getTenant(id: string) {
    return this.http.get<{ IS_SUCCESS: boolean; RESPONSE: Tenant[] }>(
      protectedResources.bohoTenantsApi.endpoint + '/' + id
    );
  }
  updateTenant(tenant: Tenant) {
    return this.http.put(protectedResources.bohoTenantsApi.endpoint, tenant);
  }
}
