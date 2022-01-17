import { RouteInfo } from './vertical-sidebar.metadata';

export const ROUTES: RouteInfo[] = [
  {
    path: '/dashboard',
    title: 'Dashboard',
    icon: 'Home',
    class: '',
    extralink: false,
    submenu: []
  },
  {
    path: '',
    title: 'Administration',
    icon: 'Cpu',
    class: 'has-arrow',
    extralink: false,
    submenu: [
      {
        path: '/administration/tenants',
        title: 'Tenants',
        icon: 'mdi mdi-adjust',
        class: '',
        extralink: false,
        submenu: []
      },
      {
        path: '/administration/widgets',
        title: 'Widgets',
        icon: 'mdi mdi-adjust',
        class: '',
        extralink: false,
        submenu: []
      },
      {
        path: '/administration/useraccounts',
        title: 'User Accounts',
        icon: 'mdi mdi-adjust',
        class: '',
        extralink: false,
        submenu: []
      }
    ]
  }
];
