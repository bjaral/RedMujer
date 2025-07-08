import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { MATERIAL_IMPORTS } from '../../shared/material/material';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-sidebar-emp',
  imports: [RouterModule, MATERIAL_IMPORTS, CommonModule],
  templateUrl: './sidebar-emp.component.html',
  styleUrl: './sidebar-emp.component.scss'
})

export class SidebarEmpComponent {

  isMobileMenuOpen: boolean = false;

  constructor(private router: Router, private authService: AuthService) { }

  toggleMobileMenu(): void {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  confirmarLogout() {
    const confirmacion = confirm('¿Estás segura de que deseas cerrar sesión?');
    if (confirmacion) {
      this.authService.logout();
      this.router.navigate(['/']);
    }
  }

}
