import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BreakpointObserver } from '@angular/cdk/layout';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { StepperOrientation } from '@angular/material/stepper';
import { AuthService } from '../../services/auth.service';
import { UbicacionService } from '../../services/ubicacion.service';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, ...MATERIAL_IMPORTS, FormsModule, ReactiveFormsModule],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.scss',
  providers: [
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { 
        displayDefaultIndicatorType: false,
        showError: true
      }
    }
  ]
})
export class RegistroComponent {
  stepperOrientation: Observable<StepperOrientation>;
  regiones: string[] = [];
  comunas: string[] = [];

  usuarioForm: any;
  personalesForm: any;
  ubicacionForm: any;

  constructor(private fb: FormBuilder, private authService: AuthService) {
    const breakpointObserver = inject(BreakpointObserver);
    this.stepperOrientation = breakpointObserver
      .observe('(min-width: 800px)')
      .pipe(map(({ matches }) => (matches ? 'horizontal' : 'vertical')));

    // Inicializa los formularios aquí
    this.usuarioForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    }, { validators: this.passwordMatchValidator });

    this.personalesForm = this.fb.group({
      run: ['', Validators.required],
      nombre: ['', Validators.required],
      apellido1: ['', Validators.required],
      apellido2: ['', Validators.required],
    });

    this.ubicacionForm = this.fb.group({
      region: ['', Validators.required],
      comuna: ['', Validators.required],
      calle: ['', Validators.required],
      numero: ['', Validators.required],
      referencia: [''],
    });

    this.regiones 
  }

  // onRegionChange() {
  //   const regionSeleccionada = this.ubicacionForm.get('region')?.value;
  //   const region = this.regiones.find(r => r.nombre === regionSeleccionada);
  //   this.comunas = region ? region.comunas : [];
  //   this.ubicacionForm.get('comuna')?.setValue('');
  // }

  ngOnInit() {
    
  }

  passwordMatchValidator(form: any) {
    return form.get('password')?.value === form.get('confirmPassword')?.value
      ? null : { mismatch: true };
  }

  // Método para prevenir clics en el header del stepper
  onStepHeaderClick(event: Event) {
    event.preventDefault();
    event.stopPropagation();
    return false;
  }

  onSubmit() {
    if (
      this.usuarioForm.valid &&
      this.personalesForm.valid &&
      this.ubicacionForm.valid
    ) {
      const data = {
        usuario: {
          username: this.usuarioForm.value.username,
          email: this.usuarioForm.value.email,
          password: this.usuarioForm.value.password
        },
        persona: this.personalesForm.value,
        ubicacion: this.ubicacionForm.value
      };

      this.authService.register(data).subscribe({
        next: (res) => {
          console.log('Registro exitoso', res);
        },
        error: (err) => {
          console.error('Error en registro', err)
        }
      });
    }
  }
}