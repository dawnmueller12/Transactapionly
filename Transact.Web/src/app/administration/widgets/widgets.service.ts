import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { protectedResources } from 'src/app/auth-config';
import { Widget } from './widget';

@Injectable()
export class WidgetsService {
  constructor(private http: HttpClient) {}

  public getWidgets() {
    return this.http.get<{ IS_SUCCESS: boolean; RESPONSE: Widget[] }>(
      protectedResources.bohoWidgetsApi.endpoint
    );
  }
  getRolesByWidgetId(id: string) {
    return this.http.get<{ IS_SUCCESS: boolean; RESPONSE: Widget }>(
      protectedResources.bohoWidgetsApi.endpoint + '/' + id + '/roles'
    );
  }
  addRoleToWidget(data: { widgetId: string; roleId: string }) {
    return this.http.post(
      protectedResources.bohoWidgetsApi.endpoint + '/AddRoles',
      { widgetId: data.widgetId, roleIds: [data.roleId] }
    );
  }
  deleteWidgetRole(data: { widgetId: string; roleId: string }) {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: { widgetId: data.widgetId, roleIds: [data.roleId] },
    };
    return this.http.delete(
      protectedResources.bohoWidgetsApi.endpoint + '/DeleteRoles',
      options
    );
  }
}
