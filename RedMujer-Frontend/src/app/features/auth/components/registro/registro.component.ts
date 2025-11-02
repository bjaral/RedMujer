import { Component, inject, OnInit, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, FormsModule, ReactiveFormsModule, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { BreakpointObserver } from '@angular/cdk/layout';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { StepperOrientation } from '@angular/material/stepper';
import { Router } from '@angular/router';

import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { AuthService } from '../../../../core/services/auth.service';
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
export class RegistroComponent implements OnInit, AfterViewInit {
  @ViewChild('tituloRef') tituloRef!: ElementRef<HTMLHeadingElement>;
  @ViewChild('contenedorRef') contenedorRef!: ElementRef<HTMLElement>;
  @ViewChild('parrafoRef') parrafoRef!: ElementRef<HTMLElement>;

  stepperOrientation: Observable<StepperOrientation>;
  regiones: any[] = [];
  comunas: any[] = [];

  usuarioForm: any;
  personalesForm: any;
  ubicacionForm: any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private ubicacionService: UbicacionService,
    private router: Router
  ) {
    const breakpointObserver = inject(BreakpointObserver);
    this.stepperOrientation = breakpointObserver
      .observe('(min-width: 800px)')
      .pipe(map(({ matches }) => (matches ? 'horizontal' : 'vertical')));

    this.usuarioForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email], [this.emailExistsValidator()]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    }, { validators: this.passwordMatchValidator });

    this.personalesForm = this.fb.group({
      run: ['', [Validators.required, this.rutValidator()]],
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

  ngAfterViewInit(): void {
    if (this.tituloRef && this.contenedorRef && this.parrafoRef) {
      const h2Width = this.tituloRef.nativeElement.offsetWidth;
      this.contenedorRef.nativeElement.style.width = `${h2Width}px`;
      this.parrafoRef.nativeElement.style.width = `${h2Width}px`;

      const ilustracionDiv = this.contenedorRef.nativeElement.querySelector('.registro-ilustracion');
      if (ilustracionDiv) {
        (ilustracionDiv as HTMLElement).style.width = `${h2Width}px`;
      }
    }
  }

  formatRut(event: any): void {
    let value = event.target.value;

    value = value.replace(/[^0-9kK]/g, '');

    let formattedRut = '';
    if (value.length > 1) {
      let rutBody = value.substring(0, value.length - 1);
      const dv = value.substring(value.length - 1).toUpperCase();

      rutBody = rutBody.replace(/\B(?=(\d{3})+(?!\d))/g, '.');

      formattedRut = `${rutBody}-${dv}`;
    } else {
      formattedRut = value;
    }

    event.target.value = formattedRut;

    this.personalesForm.get('run')?.setValue(formattedRut, { emitEvent: false });
  }

  rutValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const rut: string = control.value;

      if (!rut) {
        return null;
      }

      const cleanRut: string = rut.replace(/\.|-/g, "");

      if (cleanRut.length < 8) {
        return null;
      }

      const rutRegex: RegExp = /^\d{1,2}\.\d{3}\.\d{3}-[\dkK]$/;

      if (!rutRegex.test(rut.trim())) {
        return { invalidRutFormat: true };
      }

      const checkDigit: string = cleanRut.slice(-1).toLowerCase();
      const rutNumber: string = cleanRut.slice(0, -1);

      let sum: number = 0;
      let multiplier: number = 2;

      const reversedRut: string[] = rutNumber.split("").reverse();
      for (let i: number = 0; i < reversedRut.length; i++) {
        sum += parseInt(reversedRut[i]) * multiplier;
        multiplier = multiplier >= 7 ? 2 : multiplier + 1;
      }

      const remainder: number = sum % 11;
      let expectedDigit: string;

      if (remainder === 0) {
        expectedDigit = "0";
      } else if (remainder === 1) {
        expectedDigit = "k";
      } else {
        expectedDigit = (11 - remainder).toString();
      }

      if (expectedDigit !== checkDigit) {
        return { invalidRutChecksum: true };
      }

      return null;
    };
  }

  emailExistsValidator(): any {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (!control.value) {
        return new Observable(observer => {
          observer.next(null);
          observer.complete();
        });
      }

      const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
      if (!emailPattern.test(control.value)) {
        return new Observable(observer => {
          observer.next(null);
          observer.complete();
        });
      }

      return new Observable(observer => {
        const timeoutId = setTimeout(() => {
          this.authService.verificarCorreo(control.value).subscribe({
            next: (response) => {
              if (response.existe) {
                observer.next({ emailExists: true });
              } else {
                observer.next(null);
              }
              observer.complete();
            },
            error: () => {
              observer.next(null);
              observer.complete();
            }
          });
        }, 500);

        return () => clearTimeout(timeoutId);
      });
    };
  }

  passwordMatchValidator(form: any) {
    return form.get('password')?.value === form.get('confirmPassword')?.value
      ? null : { mismatch: true };
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
          RUN: this.personalesForm.value.run,
          Nombre: this.personalesForm.value.nombre,
          PrimerApellido: this.personalesForm.value.apellido1,
          SegundoApellido: this.personalesForm.value.apellido2,
          Vigencia: true,
          Id_Usuario: 0,
          Id_Ubicacion: 0
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
          console.log('Registro exitoso', res);
          this.toInicio();
        },
        error: (err) => {
          console.error('Error en registro:', err);
          alert('Ocurrió un error en registro.');
        }
      });
    } else {
      this.usuarioForm.markAllAsTouched();
      this.personalesForm.markAllAsTouched();
      this.ubicacionForm.markAllAsTouched();
      console.log('Formulario inválido', this.usuarioForm.errors, this.personalesForm.errors, this.ubicacionForm.errors);
    }
  }

  toInicio() {
    this.router.navigate(['']);
  }
}
