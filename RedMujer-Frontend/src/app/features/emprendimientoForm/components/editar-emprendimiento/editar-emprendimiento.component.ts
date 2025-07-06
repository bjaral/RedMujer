import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { EmprendimientoFormService } from '../../services/emprendimiento-form.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-nuevo-emprendimiento',
  standalone: true,
  imports: [MATERIAL_IMPORTS, ReactiveFormsModule, CommonModule],
  templateUrl: './editar-emprendimiento.component.html',
  styleUrl: './editar-emprendimiento.component.scss'
})

export class EditarEmprendimientoComponent implements OnInit {

  formulario: FormGroup;
  imagenSeleccionada: string | ArrayBuffer | null = null;
  imagenesExtras: string[] = [];
  imagenPrincipalFile: File | null = null;
  imagenesExtrasFile: File[] = [];
  idEmprendimiento!: number; 

  constructor(private fb: FormBuilder, private emprendimientoFormService: EmprendimientoFormService, private router: Router, private route: ActivatedRoute) {
    this.formulario = this.fb.group({
      rut: ['', Validators.required],
      nombre: ['', Validators.required],
      descripcion: [''],
      modalidad: ['', Validators.required],
      horario_Atencion: ['', Validators.required],
      imagen: ['']
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.idEmprendimiento = +params['id'];
      if (this.idEmprendimiento) {
        this.cargarEmprendimiento(this.idEmprendimiento);
      }
    });
  }

  cargarEmprendimiento(id: number): void {
    this.emprendimientoFormService.obtenerEmprendimientoPorId(id).subscribe({
      next: (emprendimiento) => {
        this.formulario.patchValue({
          rut: emprendimiento.rut,
          nombre: emprendimiento.nombre,
          descripcion: (emprendimiento.descripcion && emprendimiento.descripcion !== 'null') ? emprendimiento.descripcion : '',
          modalidad: emprendimiento.modalidad,
          horario_Atencion: emprendimiento.horario_Atencion,
        });
        if (emprendimiento.imagen) {
          this.imagenSeleccionada = encodeURI(`http://localhost:5145/media/${emprendimiento.imagen}`);
        }
      },
      error: (error) => {
        console.error('Error al cargar el emprendimiento', error);
        alert('Ocurrió un error al cargar el emprendimiento. Por favor, intenténtelo de nuevo');
        this.router.navigate(['/mis-emprendimientos']);
      }
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

    const data = this.formulario.value;
    const formData = new FormData();

    formData.append('RUT', data.rut);
    formData.append('Nombre', data.nombre);
    formData.append('Descripcion', data.descripcion || '');
    formData.append('Modalidad', data.modalidad);
    formData.append('Horario_Atencion', data.horario_Atencion);
    formData.append('Vigencia', 'true');

    // Solo agregá la imagen si el usuario seleccionó una nueva
    if (this.imagenPrincipalFile) {
      formData.append('Imagen', this.imagenPrincipalFile);
    }

    this.emprendimientoFormService.actualizarEmprendimiento(this.idEmprendimiento, formData).subscribe({
      next: () => {
        alert('Emprendimiento actualizado correctamente.');
        this.router.navigate(['/mis-emprendimientos']);
      },
      error: (error) => {
        console.error('Error al actualizar el emprendimiento', error);
        alert('Ocurrió un error al actualizar el emprendimiento.');
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
