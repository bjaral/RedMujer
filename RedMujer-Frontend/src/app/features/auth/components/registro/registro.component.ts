import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BreakpointObserver } from '@angular/cdk/layout';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { StepperOrientation } from '@angular/material/stepper';

// Valores temporales para probar
const REGIONES = [
  { nombre: 'Región Metropolitana', comunas: ['Santiago', 'Providencia', 'Las Condes'] },
  { nombre: 'Valparaíso', comunas: ['Valparaíso', 'Viña del Mar', 'Quilpué'] },
];

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, ...MATERIAL_IMPORTS, FormsModule, ReactiveFormsModule],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.scss',
})
export class RegistroComponent {
  stepperOrientation: Observable<StepperOrientation>;
  regiones = REGIONES;
  comunas: string[] = [];

  usuarioForm: any;
  personalesForm: any;
  ubicacionForm: any;

  constructor(private fb: FormBuilder) {
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
  }

  onRegionChange() {
    const regionSeleccionada = this.ubicacionForm.get('region')?.value;
    const region = this.regiones.find(r => r.nombre === regionSeleccionada);
    this.comunas = region ? region.comunas : [];
    this.ubicacionForm.get('comuna')?.setValue('');
  }

  passwordMatchValidator(form: any) {
    return form.get('password')?.value === form.get('confirmPassword')?.value
      ? null : { mismatch: true };
  }

  onSubmit() {
    if (
      this.usuarioForm.valid &&
      this.personalesForm.valid &&
      this.ubicacionForm.valid
    ) {
      // Aquí puedes manejar el envío del formulario
      console.log({
        ...this.usuarioForm.value,
        ...this.personalesForm.value,
        ...this.ubicacionForm.value,
      });
    }
  }
}