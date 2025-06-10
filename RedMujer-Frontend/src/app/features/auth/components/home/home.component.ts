import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { HeaderComponent } from '../../../../layout/header/header.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    HeaderComponent
  ],
  template: `
    <app-header></app-header>

    <section class="hero">
      <div class="hero-text">
        <h1>Impulsa tu emprendimiento</h1>
        <p>RedMujer es una plataforma que conecta, apoya y visibiliza a emprendedoras de todo el país.</p>
        <button mat-raised-button color="accent" routerLink="/registro">¡Comienza ahora!</button>
      </div>
    </section>

    <section class="emprendimientos-section">
      <h2>Conoce algunos emprendimientos</h2>
      <div class="emprendimientos-grid">
        <mat-card class="emprendimiento-card" *ngFor="let emp of emprendimientos">
          <div class="imagen-ficticia"></div>
          <mat-card-title>{{ emp.nombre }}</mat-card-title>
          <mat-card-content>{{ emp.descripcion }}</mat-card-content>
        </mat-card>
      </div>
    </section>
  `,
  styles: [`
    .hero {
      display: flex;
      justify-content: center;
      align-items: center;
      padding: 4rem 1rem;
      background: linear-gradient(to bottom, #e0f7fa, #ffffff);
      text-align: center;
    }

    .hero-text {
      max-width: 600px;
    }

    .hero h1 {
      font-size: 2.5rem;
      margin-bottom: 1rem;
    }

    .hero p {
      font-size: 1.2rem;
      margin-bottom: 2rem;
    }

    .emprendimientos-section {
      padding: 3rem 1rem;
      background-color: #f5f5f5;
      text-align: center;
    }

    .emprendimientos-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 1.5rem;
      margin-top: 2rem;
    }

    .emprendimiento-card {
      padding: 1rem;
    }

    .imagen-ficticia {
      height: 120px;
      background-color: #cfd8dc;
      border-radius: 4px;
      margin-bottom: 1rem;
    }
  `]
})
export default class HomeComponent {
  emprendimientos = [
    { nombre: 'EcoModa', descripcion: 'Moda sostenible hecha por mujeres.' },
    { nombre: 'Sabores del Valle', descripcion: 'Productos gourmet naturales.' },
    { nombre: 'Manos Creativas', descripcion: 'Artesanía local y única.' },
    { nombre: 'Artes del Sur', descripcion: 'Piezas tradicionales y modernas.' }
  ];
}
