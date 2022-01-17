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
import { WidgetsService } from './widgets.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Widget } from './widget';
import { Role } from '../useraccounts/useraccount';
import { UserAccountsService } from '../useraccounts/useraccounts.service';

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
  selector: 'app-widgets',
  templateUrl: './widgets.component.html',
  styleUrls: ['./widgets.component.scss'],
})
export class WidgetsComponent implements OnInit {
  widgetList = this.widgetsService.getWidgets();
  sortWidgetList: Widget[] | null = null;
  filterWidget: Widget[] | null = null;
  cfilterWidget: Widget[] | null = null;
  page = 1;
  pageSize = 2;
  editUserAccount: FormGroup = Object.create(null);
  widgetRoles: FormGroup = Object.create(null);
  editAddLabel: string = 'Edit';
  widgetDetail: Widget | null = null;
  totalLengthOfCollection: number = 0;
  cfilterRoles: Role[] = [];
  roles: Role[] = [];
  cpage = 1;
  cpageSize = 4;

  //Sorting purpose...
  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader> =
    Object.create(null);

  constructor(
    private widgetsService: WidgetsService,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private userAccountsService: UserAccountsService
  ) {}

  ngOnInit() {
    this.editUserAccount = this.fb.group({
      Name: ['', Validators.required],
    });
    this.widgetRoles = this.fb.group({
      widgetId: [''],
      roleId: ['', Validators.required],
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
    this.widgetsService.getWidgets().subscribe(
      ({ IS_SUCCESS, RESPONSE }) => {
        if (IS_SUCCESS) {
          this.filterWidget = RESPONSE;
          this.cfilterWidget = RESPONSE;
          this.sortWidgetList = RESPONSE;
          this.totalLengthOfCollection = this.cpageSize =
            this.cfilterWidget.length;
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
      // this.sortWidgetList = this.widgetsService.getWidgets();
      // this.cfilterWidget= this.widgetsService.getWidgets();
    } else {
      /*  this.sortClientList = [...this.tableService.getTable()].sort((a, b) => {
        const res = compare(a[column] ,b?[column]);
        return direction === 'asc' ? res : -res;
      });
      this.cfilterWidget = this.cfilterWidget?.sort((a, b) => {
        const res = compare(a[column], b[column]);
        return direction === 'asc' ? res : -res;*/
    } //);
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

  openModal(targetModal: NgbModal, widget: any) {
    this.modalService.open(targetModal, {
      centered: true,
      backdrop: 'static',
    });

    if (typeof widget === 'string') {
      this.widgetRoles.patchValue({
        widgetId: widget,
      });
      this.associatedRolesRefresh(widget);
    }
  }

  closeBtnClick() {
    this.modalService.dismissAll();
    this.ngOnInit();
  }

  onSubmit() {
    this.ngOnInit();
  }
  associatedRoles(roleId: string | null) {
    let submitObservable;
    if (roleId) {
      submitObservable = this.widgetsService.deleteWidgetRole({
        widgetId: this.widgetRoles.value.widgetId,
        roleId,
      });
    } else {
      submitObservable = this.widgetsService.addRoleToWidget(
        this.widgetRoles.value
      );
    }
    submitObservable.subscribe(
      (data) => {
        this.associatedRolesRefresh(this.widgetRoles.value.widgetId);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  associatedRolesRefresh(user: string) {
    this.widgetsService.getRolesByWidgetId(user).subscribe(
      ({ IS_SUCCESS, RESPONSE }) => {
        if (IS_SUCCESS && RESPONSE.Roles) this.cfilterRoles = RESPONSE.Roles;
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
