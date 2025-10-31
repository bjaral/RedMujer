import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ...MATERIAL_IMPORTS, RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements AfterViewInit {
  @ViewChild('tituloRef') tituloRef!: ElementRef<HTMLHeadingElement>;
  @ViewChild('contenedorRef') contenedorRef!: ElementRef<HTMLElement>;
  @ViewChild('parrafoRef') parrafoRef!: ElementRef<HTMLElement>;

  hidePassword = true;
  loginForm: any;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(5)]],
    });
  }

  ngAfterViewInit(): void {
    if (this.tituloRef && this.contenedorRef && this.parrafoRef) {
      const h2Width = this.tituloRef.nativeElement.offsetWidth;

      this.contenedorRef.nativeElement.style.width = `${h2Width}px`;
      this.parrafoRef.nativeElement.style.width = `${h2Width}px`;

      const ilustracionDiv = this.contenedorRef.nativeElement.querySelector('.login-ilustracion');
      if (ilustracionDiv) {
        (ilustracionDiv as HTMLElement).style.width = `${h2Width}px`;
      }
    }
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const credentials = {
        correo: this.loginForm.value.email,
        password: this.loginForm.value.password
      };

      this.authService.login(credentials).subscribe({
        next: (res) => {
          if (res && res.token) {
            localStorage.setItem('tokenRedMujer', res.token);
          } else {
            
          }
          console.log('Inicio de sesión exitoso');
          this.toPerfil();
        },
        error: (err) => {
          alert('Error en el inicio de sesión');
        }
      });
    } else {
      console.error('Formulario no válido');
    }
  }

  toPerfil() {
    // this.router.navigate(['/perfil']);
    this.router.navigate(['/mis-emprendimientos'])
  }
}