import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MATERIAL_IMPORTS } from '../../shared/material/material';
import { TokenService } from '../../core/services/token.service';

@Component({
  selector: 'app-header',
  imports: [CommonModule, RouterModule, ...MATERIAL_IMPORTS],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  isMobileMenuOpen: boolean = false;

  constructor(
    private tokenService: TokenService,
    private router: Router
  ) {}

  toggleMobileMenu(): void {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  get isLoggedIn(): boolean {
    return this.tokenService.isLoggedIn();
  }

  get userName(): string | null {
    return this.tokenService.getUserName();
  }

  logout(): void {
    this.tokenService.clearToken();
    if (this.isMobileMenuOpen) {
      this.toggleMobileMenu();
    }
    this.router.navigate(['/']);
  }
}
