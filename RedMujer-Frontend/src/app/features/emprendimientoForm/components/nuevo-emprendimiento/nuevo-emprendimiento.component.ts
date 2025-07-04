import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { EmprendimientoFormService } from '../../services/emprendimiento-form.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nuevo-emprendimiento',
  standalone: true,
  imports: [MATERIAL_IMPORTS, ReactiveFormsModule, CommonModule],
  templateUrl: './nuevo-emprendimiento.component.html',
  styleUrl: './nuevo-emprendimiento.component.scss'
})

export class NuevoEmprendimientoComponent {

  formulario: FormGroup;
  imagenSeleccionada: string | ArrayBuffer | null = null;
  imagenesExtras: string[] = [];
  imagenPrincipalFile: File | null = null;
  imagenesExtrasFile: File[] = [];

  constructor(private fb: FormBuilder, private emprendimientoFormService: EmprendimientoFormService, private router: Router) {
    this.formulario = this.fb.group({
      rut: ['', Validators.required],
      nombre: ['', Validators.required],
      descripcion: [''],
      modalidad: ['', Validators.required],
      horario_Atencion: ['', Validators.required],
      imagen: ['']
    });
  }

  onFileSelected(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.imagenPrincipalFile = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.imagenSeleccionada = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  removeImagenPrincipal(): void {
    this.imagenPrincipalFile = null;
    this.imagenSeleccionada = null;
  }

  onExtraImagesSelected(event: Event): void {
    const files = (event.target as HTMLInputElement).files;
    if (files) {
      const selectedCount = files.length;
      const currentCount = this.imagenesExtras.length;
      if (currentCount + selectedCount > 5) {
        alert('Solo puedes subir un máximo de 5 imágenes adicionales.');
        return;
      }

      Array.from(files).forEach((file) => {
        const reader = new FileReader();
        reader.onload = () => {
          if (reader.result) {
            this.imagenesExtras.push(reader.result as string);
            this.imagenesExtrasFile.push(file as File);
          }
        };
        reader.readAsDataURL(file);
      });
    }
  }

  removeExtraImage(index: number): void {
    this.imagenesExtras.splice(index, 1);
    this.imagenesExtrasFile.splice(index, 1);
  }

  onSubmit() {
    if (this.formulario.invalid) {
      alert('Por favor completa todos los campos obligatorios.');
      return;
    }

    const formData = this.formulario.value;
    const imagenInput = document.querySelector<HTMLInputElement>('input[type="file"]:not([multiple])');
    const imagenPrincipal = imagenInput?.files?.[0] ?? null;

    this.emprendimientoFormService.crearEmprendimiento(formData, imagenPrincipal).subscribe({
      next: (response) => {
        const idEmprendimiento = response?.id_Emprendimiento;
        if (!idEmprendimiento) {
          alert('Error: no se recibió el ID del emprendimiento creado.');
          return;
        }

        if (this.imagenesExtrasFile.length > 0) {
          this.emprendimientoFormService.subirMultimedia(idEmprendimiento, this.imagenesExtrasFile).subscribe({
            next: () => {
              alert('Emprendimiento creado y multimedia subida exitosamente.');
              this.limpiarFormulario();
              this.router.navigate(['/perfil']);
            },
            error: (err) => {
              console.error('Error al subir multimedia', err);
              alert('Emprendimiento creado, pero hubo un error al subir las imágenes adicionales.');
              this.router.navigate(['/perfil']);
            }
          });
        } else {
          alert('Emprendimiento creado exitosamente.');
          this.limpiarFormulario();
          this.router.navigate(['/perfil']);
        }
      },
      error: (error) => {
        console.error('Error al crear el emprendimiento', error);
        alert('Ocurrió un error al crear el emprendimiento.');
      }
    });
  }


  private limpiarFormulario(): void {
    this.formulario.reset();
    this.imagenSeleccionada = null;
    this.imagenPrincipalFile = null;
    this.imagenesExtras = [];
    this.imagenesExtrasFile = [];
  }

}
