import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/components/login/login.component';

export const routes: Routes = [
    { 'path': 'login', 'component': LoginComponent },
    {
  path: '',
  loadComponent: () => import('./features/auth/components/home/home.component').then(m => m.default)
},
{
  path: 'registro',
  loadComponent: () =>
    import('./features/auth/components/registro/registro.component').then(m => m.default)
}
];
