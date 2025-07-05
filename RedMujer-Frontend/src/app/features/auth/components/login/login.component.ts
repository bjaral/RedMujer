import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [MatButtonModule, MatFormFieldModule, MatInputModule, MatIconModule, RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  hidePassword = true;
  loginForm: any;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(5)]],
    })
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const credentials = {
        correo: this.loginForm.value.email,    
        password: this.loginForm.value.password
      };
      console.log('Se envía al backend:', credentials);

      this.authService.login(credentials).subscribe({
        next: (res) => {
          if (res && res.token) {
            localStorage.setItem('tokenRedMujer', res.token);
          } else {

          }
          console.log('Inicio de sesión exitoso');
          this.toInicio();
        },
        error: (err) => {
          alert('Error en el inicio de sesión');
        }
      });
    } else {
      console.error('Formulario no válido');
    }
  }

  toInicio() {
    this.router.navigate(['']);
  }
}
