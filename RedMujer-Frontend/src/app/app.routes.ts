import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/components/login/login.component';
import { HomeComponent } from './features/home/components/home/home.component';
import { LayoutComponent } from './layout/layout/layout.component';
import { EmprendimientosComponent } from './features/emprendimientos/components/emprendimientos/emprendimientos.component';
import { RegistroComponent } from './features/auth/components/registro/registro.component';
import { NuevoEmprendimientoComponent } from './features/emprendimientoForm/components/nuevo-emprendimiento/nuevo-emprendimiento.component';
import { EditarEmprendimientoComponent } from './features/emprendimientoForm/components/editar-emprendimiento/editar-emprendimiento.component';
import { SidebarEmpComponent } from './layout/sidebar-emp/sidebar-emp.component';
import { LayoutHeaderComponent } from './layout/layout-header/layout-header.component';
import { PerfilComponent } from './features/profile/components/perfil/perfil.component';
import { MisEmprendimientosComponent } from './features/profile/components/mis-emprendimientos/mis-emprendimientos.component';

export const routes: Routes = [

    // Vistas con layout
    { 'path': '', 'component': LayoutComponent, 'children': [
        { 'path': '', 'component': HomeComponent },
        { 'path': 'emprendimientos', 'component': EmprendimientosComponent },
    ]},

    // Vistas sólo con header
    { 'path': '', 'component': LayoutHeaderComponent, 'children': [
        { 'path': 'registro', 'component': RegistroComponent },
        { 'path': 'login', 'component': LoginComponent },
    ]},
    { 'path': '', 'component': SidebarEmpComponent, 'children': [
        { 'path': 'nuevo-emprendimiento', 'component': NuevoEmprendimientoComponent },
        { 'path': 'editar-emprendimiento', 'component': EditarEmprendimientoComponent },
        { 'path': 'perfil', 'component': PerfilComponent},
        { 'path': 'mis-emprendimientos', 'component': MisEmprendimientosComponent}
    ]},

    //  Vistas sin layout
    

];
