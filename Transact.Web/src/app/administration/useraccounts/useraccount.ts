import { Tenant } from '../tenants/tenant';

export class UserAccount {
  public Id: string | null = null;
  public LastName: string | null = null;
  public FirstName: string | null = null;
  public MiddleName: string | null = null;
  public RoleId: string | null = null;
  public Email: string | null = null;
  public Tenants?: Tenant[] = [];
}
export class User {
  public id: string | null = null;
  public lastName: string | null = null;
  public firstName: string | null = null;
  public password: string | null = null;
  public middleName: string | null = null;
  public roleId: number | null = null;
  public email: string | null = null;
}

export interface Role {
  Id: string;
  Name: string;
}
