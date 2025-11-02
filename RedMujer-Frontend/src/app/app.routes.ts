import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
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
import { DetalleEmprendimientoComponent } from './features/emprendimientos/components/detalle-emprendimiento/detalle-emprendimiento.component';
import { CreditosComponent } from './features/creditos/creditos.component';
import { NotFoundComponent } from './features/not-found/not-found.component';

export const routes: Routes = [

    // Vistas con layout
    { 'path': '', 'component': LayoutComponent, 'children': [
        { 'path': '', 'component': HomeComponent },
        { 'path': 'emprendimientos', 'component': EmprendimientosComponent },
        { 'path': 'emprendimientos/:id', 'component': DetalleEmprendimientoComponent},
        { 'path': '404', 'component': NotFoundComponent },
        { 'path': 'creditos', 'component': CreditosComponent },
    ]},

    // Vistas sólo con header
    { 'path': '', 'component': LayoutHeaderComponent, 'children': [
        { 'path': 'registro', 'component': RegistroComponent },
        { 'path': 'login', 'component': LoginComponent },
    ]},

    //Vistas con sidebar de emprendimiento
    { 'path': '', 'component': SidebarEmpComponent, canActivateChild: [AuthGuard], 'children': [
        { 'path': 'crear-emprendimiento', 'component': NuevoEmprendimientoComponent },
        { 'path': 'editar-emprendimiento/:id', 'component': EditarEmprendimientoComponent },
        { 'path': 'perfil', 'component': PerfilComponent},
        { 'path': 'mis-emprendimientos', 'component': MisEmprendimientosComponent}
    ]},

    //  Vistas sin layout
    
    { 'path': '**', 'redirectTo': '/404' }

];
