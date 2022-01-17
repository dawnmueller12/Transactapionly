import {
  Component,
  AfterViewInit,
  EventEmitter,
  Output,
  Input,
} from '@angular/core';
import {
  NgbModal,
  ModalDismissReasons,
  NgbPanelChangeEvent,
  NgbCarouselConfig,
} from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { TranslateService } from '@ngx-translate/core';

declare var $: any;

@Component({
  selector: 'app-horizontal-navigation',
  templateUrl: './horizontal-navigation.component.html',
})
export class HorizontalNavigationComponent implements AfterViewInit {
  @Output() toggleSidebar = new EventEmitter<void>();
  @Input() user = {
    username: '',
    name: '',
  };
  public LoggedInUser = {};

  public config: PerfectScrollbarConfigInterface = {};

  public showSearch = false;

  public isCollapsed = false;
  public showMobileMenu = false;

  constructor(
    private modalService: NgbModal,
    private translate: TranslateService
  ) {
    translate.setDefaultLang('en');
  }

  ngAfterViewInit() {}

  changeLanguage(lang: any) {
    this.translate.use(lang.code);
  }
}
