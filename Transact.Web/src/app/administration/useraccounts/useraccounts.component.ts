import {
  Component,
  OnInit,
  Directive,
  EventEmitter,
  Input,
  Output,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { UserAccountsService } from './useraccounts.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Role, UserAccount } from './useraccount';
import { TenantsService } from '../tenants/tenants.service';
import { Tenant } from '../tenants/tenant';

export type SortDirection = 'asc' | 'desc' | '';
const rotate: { [key: string]: SortDirection } = {
  asc: 'desc',
  desc: '',
  '': 'asc',
};
export const compare = (v1: number, v2: number) =>
  v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

export interface SortEvent {
  column: string | null;
  direction: SortDirection;
}

@Directive({
  selector: 'th[sortable]',
  host: {
    '[class.asc]': 'direction === "asc"',
    '[class.desc]': 'direction === "desc"',
    '(click)': 'rotate()',
  },
})
export class NgbdSortableHeader {
  @Input() sortable: string | null = null;
  @Input() direction: SortDirection = '';
  @Output() sort = new EventEmitter<SortEvent>();

  rotate() {
    this.direction = rotate[this.direction];
    this.sort.emit({ column: this.sortable, direction: this.direction });
  }
}

@Component({
  selector: 'app-useraccounts',
  templateUrl: './useraccounts.component.html',
  styleUrls: ['./useraccounts.component.scss'],
})
export class UserAccountsComponent implements OnInit {
  sortUserAccountList: UserAccount[] | null = null;
  filterUserAccount: UserAccount[] | null = null;
  cfilterUserAccount: UserAccount[] | null = null;
  page = 1;
  pageSize = 2;
  editUserAccount: FormGroup = Object.create(null);
  userTenants: FormGroup = Object.create(null);
  editAddLabel: 'Add' | 'Edit' = 'Edit';
  userAccountDetail: UserAccount | null = null;
  totalLengthOfCollection: number = 0;
  roles: Role[] = [];
  tenants: Tenant[] = [];
  cfilterTenants: Tenant[] = [];
  cpage = 1;
  cpageSize = 4;

  //Sorting purpose...
  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader> =
    Object.create(null);

  constructor(
    private userAccountsService: UserAccountsService,
    private tenantsServ: TenantsService,
    private fb: FormBuilder,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    this.editUserAccount = this.fb.group({
      id: [null],
      firstName: ['', Validators.required],
      password: ['', this.editAddLabel === 'Add' && Validators.required],
      lastName: ['', Validators.required],
      middleName: [''],
      roleId: ['', [Validators.required]],
      email: ['', this.editAddLabel === 'Add' && Validators.required],
    });
    this.userTenants = this.fb.group({
      userId: [''],
      tenantId: ['', Validators.required],
    });
    this.userAccountsService.getRoles().subscribe(
      ({ IS_SUCCESS, RESPONSE }) => {
        if (IS_SUCCESS) {
          this.roles = RESPONSE;
        }
      },
      (error) => {
        console.log(error);
      }
    );
    this.userAccountsService.getUsers().subscribe(
      ({ IS_SUCCESS, RESPONSE }) => {
        if (IS_SUCCESS) {
          this.filterUserAccount = RESPONSE;
          this.cfilterUserAccount = RESPONSE;
          this.sortUserAccountList = RESPONSE;
          this.totalLengthOfCollection = this.cpageSize =
            this.cfilterUserAccount.length;
        }
      },
      (error) => {
        console.log(error);
      }
    );
    this.tenantsServ.getTenants().subscribe(
      ({ IS_SUCCESS, RESPONSE }) => {
        if (IS_SUCCESS) {
          this.tenants = RESPONSE;
        }
      },
      (error) => {
        console.log(error);
      }
    );
  }

  onSort({ column, direction }: SortEvent) {
    this.headers.forEach((header) => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    // sorting client

    if (direction === '') {
      // this.sortUserAccountList = this.userAccountsService.getUserAccounts();
      // this.cfilterUserAccount = this.userAccountsService.getUserAccounts();
    } else {
      // // //this.sortClientList = [...this.tableService.getTable()].sort((a, b) => {
      // // //  const res = compare(a[column] ,b?[column]);
      // // //  return direction === 'asc' ? res : -res;
      // // //});
      // // //this.cfilterClient = [...this.tableService.getTable()].sort((a, b) => {
      // // //  const res = compare(a[column], b[column]);
      // // //  return direction === 'asc' ? res : -res;
    } // // //);
  }

  //Model........................
  logValidationErrors(group: FormGroup) {
    // Object.keys(group.controls).forEach((key: string) => {
    //   const ac = group.get(key);
    //   this.formsErrors[key] = '';
    //   if (ac && !ac.valid && (ac.touched || ac.dirty)) {
    //     const message = this.ValidationMessage[key];
    //     for (const errorKey in ac.errors) {
    //       if (errorKey) {
    //         this.formsErrors[key] += message[errorKey] + '    ';
    //       }
    //     }
    //   }
    //   if (ac instanceof FormGroup) {
    //     this.logValidationErrors(ac)
    //   }
    // })
  }

  ValidationMessage = {
    fullName: { required: 'full Name is required.' },
    UserName: { required: 'User Name is required.' },
    email: {
      required: 'Email is required.',
      Email: 'Not a email',
    },
  };

  formsErrors = {};

  openModal(targetModal: NgbModal, user: any) {
    this.modalService.open(targetModal, {
      centered: true,
      backdrop: 'static',
    });

    if (typeof user === 'string') {
      this.userTenants.patchValue({
        userId: user,
      });
      this.associatedTenantsRefresh(user);
    } else if (user && typeof user === 'object') {
      this.editAddLabel = 'Edit';
      this.editUserAccount.patchValue({
        firstName: user.FirstName,
        lastName: user.LastName,
        middleName: user.MiddleName,
        id: user.Id,
        roleId: user.RoleId,
      });
    } else {
      this.editAddLabel = 'Add';
    }
  }

  closeBtnClick() {
    this.modalService.dismissAll();
    this.ngOnInit();
    this.cfilterTenants = [];
  }

  onSubmit() {
    let submitObservable;
    if (this.editAddLabel === 'Add') {
      submitObservable = this.userAccountsService.createUser(
        this.editUserAccount.value
      );
    } else {
      const value = this.editUserAccount.value;
      delete value.password;
      delete value.email;
      submitObservable = this.userAccountsService.updateUser(value);
    }
    submitObservable.subscribe(
      (data) => {
        this.closeBtnClick();
      },
      (error) => {
        console.log(error);
      }
    );
  }
  findRoleName(id: string) {
    return this.roles.find((role) => role.Id === id)?.Name;
  }
  associationTenant(tenantId: string | null) {
    let submitObservable;
    if (tenantId) {
      submitObservable = this.userAccountsService.deleteUserTenants({
        userId: this.userTenants.value.userId,
        tenantId,
      });
    } else {
      submitObservable = this.userAccountsService.addTenantsToUser(
        this.userTenants.value
      );
    }
    submitObservable.subscribe(
      (data) => {
        this.associatedTenantsRefresh(this.userTenants.value.userId);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  associatedTenantsRefresh(user: string) {
    this.userAccountsService.getTenantsByUserId(user).subscribe(
      ({ IS_SUCCESS, RESPONSE }) => {
        if (IS_SUCCESS && RESPONSE.Tenants)
          this.cfilterTenants = RESPONSE.Tenants;
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
