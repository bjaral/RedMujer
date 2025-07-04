import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/components/login/login.component';
import { HomeComponent } from './features/home/components/home/home.component';
import { LayoutComponent } from './layout/layout/layout.component';
import { EmprendimientosComponent } from './features/emprendimientos/components/emprendimientos/emprendimientos.component';
import { RegistroComponent } from './features/auth/components/registro/registro.component';
import { LayoutHeaderComponent } from './layout/layout-header/layout-header.component';

export const routes: Routes = [

    // Vistas con layout
    { 'path': '', 'component': LayoutComponent, 'children': [
        { 'path': '', 'component': HomeComponent },
        { 'path': 'emprendimientos', 'component': EmprendimientosComponent },
    ]},

    // Vistas sólo con header
    { 'path': '', 'component': LayoutHeaderComponent, 'children': [
        { 'path': 'registro', 'component': RegistroComponent },
    ]},

    //  Vistas sin layout
    { 'path': 'login', 'component': LoginComponent },
    

];
