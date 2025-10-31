import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { ProfileService } from '../../services/profile.service';
import { UbicacionService } from '../../services/ubicacion.service';
import { TokenService } from '../../../../core/services/token.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-perfil',
  imports: [CommonModule, ...MATERIAL_IMPORTS, ReactiveFormsModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.scss'
})
export class PerfilComponent implements OnInit {
  perfilForm!: FormGroup;
  cambiarContrasenaForm!: FormGroup;
  regiones: any[] = [];
  comunas: any[] = [];
  todasLasComunas: any[] = []; // Para buscar la región por comuna
  loading: boolean = true;
  editMode: boolean = false;
  mostrarCambiarContrasena: boolean = false;

  datosOriginales: any = null;
  personaId: number = 0;
  ubicacionId: number = 0;
  userId: number = 0;

  // Estadísticas
  totalEmprendimientos: number = 0;
  fechaRegistro: Date | null = null;

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService,
    private ubicacionService: UbicacionService,
    private tokenService: TokenService
  ) {
    // Formulario de perfil
    this.perfilForm = this.fb.group({
      username: [{ value: '', disabled: true }, Validators.required],
      email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
      run: [{ value: '', disabled: true }],
      nombre: [{ value: '', disabled: true }, Validators.required],
      apellido1: [{ value: '', disabled: true }, Validators.required],
      apellido2: [{ value: '', disabled: true }, Validators.required],
      region: [{ value: null, disabled: true }, Validators.required],
      comuna: [{ value: null, disabled: true }, Validators.required],
      calle: [{ value: '', disabled: true }, Validators.required],
      numero: [{ value: '', disabled: true }, Validators.required],
      referencia: [{ value: '', disabled: true }]
    });

    // Formulario de cambiar contraseña
    this.cambiarContrasenaForm = this.fb.group({
      contrasenaActual: ['', Validators.required],
      contrasenaNueva: ['', [Validators.required, Validators.minLength(6)]],
      confirmarContrasena: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.cargarDatosIniciales();
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('contrasenaNueva')?.value === form.get('confirmarContrasena')?.value
      ? null : { mismatch: true };
  }

  cargarDatosIniciales(): void {
    this.loading = true;
    
    // Cargar regiones y todas las comunas en paralelo
    forkJoin({
      regiones: this.ubicacionService.regiones(),
      comunas: this.ubicacionService.comunas() // Asumiendo que tienes este método
    }).subscribe({
      next: (resultado) => {
        this.regiones = resultado.regiones;
        this.todasLasComunas = resultado.comunas;
        console.log('Regiones cargadas:', this.regiones);
        console.log('Todas las comunas cargadas:', this.todasLasComunas);
        this.cargarDatosUsuario();
      },
      error: (err) => {
        console.error('Error al cargar datos iniciales', err);
        this.loading = false;
      }
    });
  }

  cargarDatosUsuario(): void {
    const userId = this.tokenService.getNameIdentifier();
    console.log('User ID from token:', userId);

    if (!userId) {
      console.error('No se encontró userId en el token');
      this.loading = false;
      return;
    }

    this.userId = userId;

    this.profileService.obtenerPerfilCompleto(userId).subscribe({
      next: (datos) => {
        console.log('Datos recibidos completos:', datos);
        console.log('Usuario:', datos.usuario);
        console.log('Persona:', datos.persona);
        console.log('Ubicación:', datos.ubicacion);

        this.datosOriginales = datos;
        this.personaId = datos.persona.id_Persona;
        this.ubicacionId = datos.persona.id_Ubicacion;

        // Cargar estadísticas
        this.profileService.contarEmprendimientos(this.personaId).subscribe({
          next: (count) => {
            this.totalEmprendimientos = count;
            console.log('Total emprendimientos:', count);
          },
          error: (err) => {
            console.error('Error al contar emprendimientos:', err);
            this.totalEmprendimientos = 0;
          }
        });

        // ENCONTRAR LA REGIÓN A TRAVÉS DE LA COMUNA
        const comunaActual = this.todasLasComunas.find(c => c.id_Comuna === datos.ubicacion.id_Comuna);
        console.log('Comuna actual encontrada:', comunaActual);

        if (comunaActual) {
          // Ahora obtenemos la región desde la comuna
          const regionActual = this.regiones.find(r => r.id_Region === comunaActual.id_Region);
          console.log('Región actual encontrada:', regionActual);

          if (regionActual) {
            // Cargar las comunas de esa región para el dropdown
            this.ubicacionService.comunasPorRegion(regionActual.id_Region).subscribe({
              next: (comunas) => {
                this.comunas = comunas;
                console.log('Comunas de la región cargadas:', comunas);

                // Ahora sí cargar los valores en el formulario
                this.perfilForm.patchValue({
                  username: datos.usuario.usuarioNombre || '',
                  email: datos.usuario.correo || '',
                  run: datos.persona.run || '',
                  nombre: datos.persona.nombre || '',
                  apellido1: datos.persona.primerApellido || '',
                  apellido2: datos.persona.segundoApellido || '',
                  region: regionActual,
                  comuna: comunaActual.nombre,
                  calle: datos.ubicacion.calle || '',
                  numero: datos.ubicacion.numero || '',
                  referencia: datos.ubicacion.referencia || ''
                });

                console.log('Valores cargados en el formulario:', this.perfilForm.getRawValue());
                this.loading = false;
              },
              error: (err) => {
                console.error('Error al cargar comunas de la región:', err);
                this.loading = false;
              }
            });
          } else {
            console.error('No se encontró la región correspondiente');
            this.loading = false;
          }
        } else {
          console.error('No se encontró la comuna en la lista');
          this.loading = false;
        }
      },
      error: (err) => {
        console.error('Error al cargar datos del usuario:', err);
        this.loading = false;
      }
    });
  }

  cargarComunasPorRegion(idRegion: number, idComunaActual?: number): void {
    this.ubicacionService.comunasPorRegion(idRegion).subscribe({
      next: (comunas) => {
        this.comunas = comunas;
        if (idComunaActual) {
          const comunaActual = this.comunas.find(c => c.id_Comuna === idComunaActual);
          if (comunaActual) {
            this.perfilForm.patchValue({ comuna: comunaActual.nombre });
          }
        }
      },
      error: (err) => {
        console.error('Error al cargar comunas', err);
      }
    });
  }

  onRegionChange(): void {
    const regionObj = this.perfilForm.get('region')?.value;
    if (regionObj && regionObj.id_Region) {
      this.cargarComunasPorRegion(regionObj.id_Region);
      this.perfilForm.patchValue({ comuna: null });
    } else {
      this.comunas = [];
      this.perfilForm.patchValue({ comuna: null });
    }
  }

  toggleEditMode(): void {
    this.editMode = !this.editMode;

    if (this.editMode) {
      // Habilitar campos editables
      this.perfilForm.get('username')?.enable();
      this.perfilForm.get('email')?.enable();
      this.perfilForm.get('nombre')?.enable();
      this.perfilForm.get('apellido1')?.enable();
      this.perfilForm.get('apellido2')?.enable();
      this.perfilForm.get('region')?.enable();
      this.perfilForm.get('comuna')?.enable();
      this.perfilForm.get('calle')?.enable();
      this.perfilForm.get('numero')?.enable();
      this.perfilForm.get('referencia')?.enable();
    } else {
      this.cancelarEdicion();
    }
  }

  cancelarEdicion(): void {
    this.editMode = false;

    // Deshabilitar campos
    this.perfilForm.get('username')?.disable();
    this.perfilForm.get('email')?.disable();
    this.perfilForm.get('nombre')?.disable();
    this.perfilForm.get('apellido1')?.disable();
    this.perfilForm.get('apellido2')?.disable();
    this.perfilForm.get('region')?.disable();
    this.perfilForm.get('comuna')?.disable();
    this.perfilForm.get('calle')?.disable();
    this.perfilForm.get('numero')?.disable();
    this.perfilForm.get('referencia')?.disable();

    // Restaurar valores originales
    if (this.datosOriginales) {
      // Encontrar la región y comuna actuales
      const comunaActual = this.todasLasComunas.find(c => c.id_Comuna === this.datosOriginales.ubicacion.id_Comuna);
      const regionActual = comunaActual ? this.regiones.find(r => r.id_Region === comunaActual.id_Region) : null;

      this.perfilForm.patchValue({
        username: this.datosOriginales.usuario.usuarioNombre,
        email: this.datosOriginales.usuario.correo,
        nombre: this.datosOriginales.persona.nombre,
        apellido1: this.datosOriginales.persona.primerApellido,
        apellido2: this.datosOriginales.persona.segundoApellido,
        region: regionActual,
        comuna: comunaActual ? comunaActual.nombre : null,
        calle: this.datosOriginales.ubicacion.calle,
        numero: this.datosOriginales.ubicacion.numero,
        referencia: this.datosOriginales.ubicacion.referencia
      });

      if (regionActual) {
        this.cargarComunasPorRegion(regionActual.id_Region, this.datosOriginales.ubicacion.id_Comuna);
      }
    }
  }

  guardarCambios(): void {
    if (this.perfilForm.valid) {
      const regionObj = this.perfilForm.get('region')?.value;
      const comunaObj = this.comunas.find(c => c.nombre === this.perfilForm.get('comuna')?.value);

      if (!comunaObj) {
        alert('Por favor selecciona una comuna válida');
        return;
      }

      // Preparar datos de Usuario
      const datosUsuario = {
        id_Usuario: this.userId,
        usuarioNombre: this.perfilForm.get('username')?.value,
        correo: this.perfilForm.get('email')?.value,
        contrasenna: this.datosOriginales.usuario.contrasenna,
        vigencia: true,
        tipo_Usuario: this.datosOriginales.usuario.tipo_Usuario
      };

      // Preparar datos de Persona
      const datosPersona = {
        id_Persona: this.personaId,
        id_Ubicacion: this.ubicacionId,
        id_Usuario: this.userId,
        run: this.datosOriginales.persona.run,
        nombre: this.perfilForm.get('nombre')?.value,
        primerApellido: this.perfilForm.get('apellido1')?.value,
        segundoApellido: this.perfilForm.get('apellido2')?.value,
        vigencia: true
      };

      // Preparar datos de Ubicación
      const datosUbicacion = {
        id_Ubicacion: this.ubicacionId,
        id_Comuna: comunaObj.id_Comuna,
        calle: this.perfilForm.get('calle')?.value,
        numero: this.perfilForm.get('numero')?.value,
        referencia: this.perfilForm.get('referencia')?.value || '',
        vigencia: true
      };

      console.log('Datos a enviar:', { datosUsuario, datosPersona, datosUbicacion });

      // Llamar al servicio para actualizar todo
      this.profileService.actualizarPerfilCompleto(
        this.userId,
        this.personaId,
        this.ubicacionId,
        datosUsuario,
        datosPersona,
        datosUbicacion
      ).subscribe({
        next: (response) => {
          console.log('Perfil actualizado correctamente', response);
          alert('Perfil actualizado exitosamente');
          this.toggleEditMode();
          this.cargarDatosUsuario();
        },
        error: (err) => {
          console.error('Error al actualizar perfil:', err);
          alert('Ocurrió un error al actualizar el perfil.');
        }
      });
    } else {
      this.perfilForm.markAllAsTouched();
      console.log('Formulario inválido');
    }
  }

  toggleCambiarContrasena(): void {
    this.mostrarCambiarContrasena = !this.mostrarCambiarContrasena;
    if (!this.mostrarCambiarContrasena) {
      this.cambiarContrasenaForm.reset();
    }
  }

  cambiarContrasena(): void {
    if (this.cambiarContrasenaForm.valid) {
      const { contrasenaActual, contrasenaNueva } = this.cambiarContrasenaForm.value;

      this.profileService.cambiarContrasena(this.userId, contrasenaActual, contrasenaNueva).subscribe({
        next: () => {
          alert('Contraseña cambiada exitosamente');
          this.toggleCambiarContrasena();
        },
        error: (err) => {
          console.error('Error al cambiar contraseña:', err);
          alert('Error al cambiar la contraseña. Verifica que la contraseña actual sea correcta.');
        }
      });
    } else {
      this.cambiarContrasenaForm.markAllAsTouched();
    }
  }
}