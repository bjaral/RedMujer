import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule, MatToolbarModule, MatButtonModule],
  template: `
    <mat-toolbar color="primary" class="header-toolbar">
      <span class="logo">RedMujer</span>
      <span class="spacer"></span>
      <button mat-button routerLink="/login">Iniciar Sesión</button>
      <button mat-raised-button color="accent" routerLink="/registro">Registrarse</button>
      
    </mat-toolbar>
  `,
  styles: [`
    .header-toolbar {
      position: sticky;
      top: 0;
      z-index: 1000;
    }

    .logo {
      font-weight: bold;
      font-size: 1.5rem;
    }

    .spacer {
      flex: 1 1 auto;
    }

    button {
      margin-left: 8px;
    }
  `]
})
export class HeaderComponent {}
