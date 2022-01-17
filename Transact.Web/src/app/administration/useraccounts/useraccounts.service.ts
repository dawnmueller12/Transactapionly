import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { protectedResources } from 'src/app/auth-config';
import { Tenant } from '../tenants/tenant';
import { Role, User, UserAccount } from './useraccount';

@Injectable()
export class UserAccountsService {
  constructor(private http: HttpClient) {}

  createUser(user: User) {
    return this.http.post(protectedResources.bohoUsersApi.endpoint, user);
  }
  getUsers() {
    return this.http.get<{ IS_SUCCESS: boolean; RESPONSE: UserAccount[] }>(
      protectedResources.bohoUsersApi.endpoint
    );
  }
  getUser(id: string) {
    return this.http.get<{ IS_SUCCESS: boolean; RESPONSE: UserAccount[] }>(
      protectedResources.bohoUsersApi.endpoint + '/' + id
    );
  }
  updateUser(user: User) {
    return this.http.put(protectedResources.bohoUsersApi.endpoint, user);
  }

  getRoles() {
    return this.http.get<{
      IS_SUCCESS: boolean;
      RESPONSE: Role[];
    }>(protectedResources.bohoRolesApi.endpoint);
  }

  getTenantsByUserId(id: string) {
    return this.http.get<{ IS_SUCCESS: boolean; RESPONSE: UserAccount }>(
      protectedResources.bohoUsersApi.endpoint + '/' + id + '/tenants'
    );
  }
  addTenantsToUser(data: { userId: string; tenantId: string }) {
    return this.http.post(
      protectedResources.bohoUsersApi.endpoint + '/AddTenants',
      { userId: data.userId, tenantIds: [data.tenantId] }
    );
  }
  deleteUserTenants(data: { userId: string; tenantId: string }) {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: { userId: data.userId, tenantIds: [data.tenantId] },
    };
    return this.http.delete(
      protectedResources.bohoUsersApi.endpoint + '/DeleteTenants',
      options
    );
  }
}
