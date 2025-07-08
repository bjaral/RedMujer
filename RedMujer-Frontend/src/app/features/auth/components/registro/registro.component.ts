import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BreakpointObserver } from '@angular/cdk/layout';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { StepperOrientation } from '@angular/material/stepper';
import { AuthService } from '../../../../core/services/auth.service';
import { UbicacionService } from '../../services/ubicacion.service';
import { Router } from '@angular/router';
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
export class RegistroComponent implements OnInit {
  stepperOrientation: Observable<StepperOrientation>;
  regiones: any[] = [];
  comunas: any[] = [];

  usuarioForm: any;
  personalesForm: any;
  ubicacionForm: any;

  constructor(private fb: FormBuilder, private authService: AuthService, private ubicacionService: UbicacionService, private router: Router) {
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
      region: [null, Validators.required],
      comuna: [null, Validators.required],
      calle: ['', Validators.required],
      numero: ['', Validators.required],
      referencia: [''],
    });

    this.regiones
  }

  ngOnInit() {
    this.ubicacionService.regiones().subscribe({
      next: (regiones) => {
        this.regiones = regiones;
      },
      error: (err) => {
        console.error('Error al cargar regiones', err);
      }
    });
  }

  onRegionChange() {
    const regionObj = this.ubicacionForm.get('region')?.value;
    if (regionObj && regionObj.id_Region) {
      console.log(regionObj.id_Region)
      this.ubicacionService.comunasPorRegion(regionObj.id_Region).subscribe({
        next: (comunas) => {
          this.comunas = comunas;
          this.ubicacionForm.get('comuna')?.setValue(null);
        },
        error: (err) => {
          console.error('Error al cargar comunas', err);
        }
      });
    } else {
      this.comunas = [];
      this.ubicacionForm.get('comuna')?.setValue(null);
    }
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
      const regionObj = this.ubicacionForm.value.region;
      const comunaObj = this.comunas.find(
        c => c.nombre === this.ubicacionForm.value.comuna
      );

      const data = {
        usuario: {
          usuario: this.usuarioForm.value.username,
          contrasenna: this.usuarioForm.value.password,
          vigencia: true,
          tipo_Usuario: 'emprendedora',
          correo: this.usuarioForm.value.email,
        },
        persona: {
          run: this.personalesForm.value.run,
          nombre: this.personalesForm.value.nombre,
          primerApellido: this.personalesForm.value.apellido1,
          segundoApellido: this.personalesForm.value.apellido2,
          vigencia: true,
        },
        ubicacion: {
          id_Region: Number(regionObj.id_Region),
          id_Comuna: Number(comunaObj.id_Comuna),
          calle: this.ubicacionForm.value.calle,
          numero: String(this.ubicacionForm.value.numero),
          referencia: this.ubicacionForm.value.referencia,
          vigencia: true,
        }

      };

      this.authService.register(data).subscribe({
        next: (res) => {
          console.log('Registro exitoso');
          this.toInicio();
        },
        error: (err) => {
          alert('Ocurrió un error en registro.')
        }
      });
    }
  }

  toInicio() {
    this.router.navigate(['']);
  }
}