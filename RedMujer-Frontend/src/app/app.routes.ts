import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/components/login/login.component';
import { HomeComponent } from './features/home/components/home/home.component';
import { LayoutComponent } from './layout/layout/layout.component';

export const routes: Routes = [

    // Vistas con layout
    { 'path': '', 'component': LayoutComponent, 'children': [
        { 'path': '', 'component': HomeComponent },
    ]},

    //  Vistas sin layout
    { 'path': 'login', 'component': LoginComponent },

];
