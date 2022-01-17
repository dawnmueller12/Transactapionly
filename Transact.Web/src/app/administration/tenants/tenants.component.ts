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
import { TenantsService } from './tenants.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Tenant } from './tenant';

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
  selector: 'app-tenants',
  templateUrl: './tenants.component.html',
  styleUrls: ['./tenants.component.scss'],
})
export class TenantsComponent implements OnInit {
  sortTenantList: Tenant[] | null = null;
  filterTenant: Tenant[] | null = null;
  cfilterTenant: Tenant[] | null = null;
  page = 1;
  pageSize = 2;
  editTenant: FormGroup = Object.create(null);
  editAddLabel: 'Add' | 'Edit' = 'Add';
  tenantDetail: Tenant | null = null;
  totalLengthOfCollection: number = 0;
  cpage = 1;
  cpageSize = 4;

  //Sorting purpose...
  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader> =
    Object.create(null);

  constructor(
    private tenantsService: TenantsService,
    private fb: FormBuilder,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    this.editTenant = this.fb.group({
      id: [null],
      organizationName: ['', Validators.required],
      organizationAddress: ['', Validators.required],
      state: ['', [Validators.required]],
      city: ['', [Validators.required]],
      pincode: ['', [Validators.required]],
    });

    this.tenantsService.getTenants().subscribe(
      ({ IS_SUCCESS, RESPONSE }) => {
        if (IS_SUCCESS) {
          this.filterTenant = RESPONSE;
          this.cfilterTenant = RESPONSE;
          this.sortTenantList = RESPONSE;
          this.totalLengthOfCollection = this.cpageSize =
            this.cfilterTenant.length;
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
      // this.sortTenantList = this.tenantsService.getTenants();
      // this.cfilterTenant = this.tenantsService.getTenants();
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

  openModal(targetModal: NgbModal, tenant: any) {
    this.modalService.open(targetModal, {
      centered: true,
      backdrop: 'static',
    });
    if (tenant == null) {
      this.editAddLabel = 'Add';
    }

    if (tenant != null) {
      this.tenantDetail = tenant;
      this.editAddLabel = 'Edit';
      this.editTenant.patchValue({
        id: tenant.Id,
        organizationName: tenant.OrganizationName,
        organizationAddress: tenant.OrganizationAddress,
        state: tenant.State,
        city: tenant.City,
        pincode: tenant.Pincode,
      });
    }
  }

  closeBtnClick() {
    this.modalService.dismissAll();
    this.ngOnInit();
  }

  onSubmit() {
    let submitObservable;
    if (this.editAddLabel === 'Add') {
      submitObservable = this.tenantsService.createTenant(
        this.editTenant.value
      );
    } else {
      submitObservable = this.tenantsService.updateTenant(
        this.editTenant.value
      );
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
}
