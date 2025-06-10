import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../../layout/header/header.component';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    HeaderComponent
  ],
  template: `
    <app-header></app-header>

    <div class="registro-container">
      <h2>Registro de nueva usuaria</h2>
      <form [formGroup]="registroForm" (ngSubmit)="onSubmit()">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Nombre completo</mat-label>
          <input matInput formControlName="nombre" required />
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Correo electrónico</mat-label>
          <input matInput formControlName="email" type="email" required />
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Contraseña</mat-label>
          <input matInput formControlName="password" type="password" required />
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Confirmar contraseña</mat-label>
          <input matInput formControlName="confirmPassword" type="password" required />
        </mat-form-field>

        <button mat-raised-button color="accent" type="submit" [disabled]="registroForm.invalid">
          Registrarse
        </button>
      </form>
    </div>
  `,
  styles: [`
    .registro-container {
      max-width: 500px;
      margin: 4rem auto;
      padding: 2rem;
      box-shadow: 0 0 10px rgba(0,0,0,0.1);
      border-radius: 8px;
      background-color: #ffffff;
    }

    .full-width {
      width: 100%;
      margin-bottom: 1rem;
    }

    h2 {
      text-align: center;
      margin-bottom: 2rem;
    }
  `]
})
export default class RegistroComponent {
  registroForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.registroForm = this.fb.group({
      nombre: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.registroForm.valid) {
      const { password, confirmPassword } = this.registroForm.value;
      if (password !== confirmPassword) {
        alert('Las contraseñas no coinciden.');
        return;
      }
      console.log('Registro enviado:', this.registroForm.value);
      // Más adelante: conectar con AuthService
    }
  }
}
