import { Role } from '../useraccounts/useraccount';

export class Widget {
  Id: string | null = null;
  public Name: string | null = null;
  public CreatedOn: Date | null = null;
  public CreatedBy: number | null = null;
  public UpdatedOn: Date | null = null;
  public UpdatedBy: number | null = null;
  public Roles: Role[] = [];
}
