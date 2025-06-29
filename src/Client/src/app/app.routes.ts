import { Routes } from '@angular/router';
import {DreamPage} from './components/dream/dream-page/dream-page';
import {DreamsList} from './components/dream/dreams-list/dreams-list';
import {Login} from './components/auth/login/login';
import {Register} from './components/auth/register/register';
import {Profile} from './components/Profile/profile/profile';

export const routes: Routes = [
  { path: '', component: DreamsList },
  { path: 'dreams/:dreamId', component: DreamPage },
  { path: 'login', component: Login },
  { path: 'reg', component: Register },
  { path: 'profile', component: Profile },
];
