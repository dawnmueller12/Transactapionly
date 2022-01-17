import {
  Component,
  AfterViewInit,
  EventEmitter,
  Output,
  Input,
} from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { TranslateService } from '@ngx-translate/core';
import { MsalService } from '@azure/msal-angular';

declare var $: any;

@Component({
  selector: 'app-vertical-navigation',
  templateUrl: './vertical-navigation.component.html',
})
export class VerticalNavigationComponent implements AfterViewInit {
  @Output() toggleSidebar = new EventEmitter<void>();
  @Input() user = {
    username: '',
    name: '',
  };

  public config: PerfectScrollbarConfigInterface = {};

  public showSearch = false;
  constructor(
    private modalService: NgbModal,
    private translate: TranslateService,
    private authService: MsalService
  ) {
    translate.setDefaultLang('en');
  }

  ngAfterViewInit() {}

  changeLanguage(lang: any) {
    this.translate.use(lang.code);
  }
  logout() {
    this.authService.logout();
  }
}
