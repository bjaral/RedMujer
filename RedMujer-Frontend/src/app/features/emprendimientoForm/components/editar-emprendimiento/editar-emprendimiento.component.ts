import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { EmprendimientoFormService } from '../../services/emprendimiento-form.service';
import { Router, ActivatedRoute } from '@angular/router';

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
  imagenesExtrasData: { preview: string; file: File }[] = [];
  imagenPrincipalFile: File | null = null;
  idEmprendimiento!: number;

  constructor(private fb: FormBuilder, private emprendimientoFormService: EmprendimientoFormService, private router: Router,private route: ActivatedRoute) {
    this.formulario = this.fb.group({
      rut: ['', Validators.required],
      nombre: ['', Validators.required],
      descripcion: [''],
      modalidad: ['', Validators.required],
      horario_Atencion: ['', Validators.required],
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
          this.imagenSeleccionada = `http://localhost:5145/media/${emprendimiento.imagen}`;
        }

        this.emprendimientoFormService.obtenerMultimediaPorId(id).subscribe({
          next: (imagenes) => {
            this.imagenesExtras = imagenes;
            this.imagenesExtrasData = [];
          },
          error: (err) => {
            console.error('Error al cargar las imágenes secundarias', err);
          }
        });
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
    const confirmar = confirm('¿Estás segura de querer eliminar la imagen principal?');
    if (!confirmar) return;

    if (this.imagenSeleccionada && typeof this.imagenSeleccionada === 'string' && this.imagenSeleccionada.startsWith('data:')) {
      this.imagenPrincipalFile = null;
      this.imagenSeleccionada = null;
      return;
    }

    this.emprendimientoFormService.borrarImagenPrincipal(this.idEmprendimiento).subscribe({
      next: () => {
        this.imagenPrincipalFile = null;
        this.imagenSeleccionada = null;
        alert('Imagen principal eliminada correctamente.');
      },
      error: (err) => {
        console.error('Error al eliminar la imagen principal', err);
        alert('Ocurrió un error al eliminar la imagen principal. Por favor, intenténtelo de nuevo.');
      }
    });
  }

  onExtraImagesSelected(event: Event): void {
    const files = (event.target as HTMLInputElement).files;
    if (files) {
      const selectedCount = files.length;
      const currentCount = this.imagenesExtras.length + this.imagenesExtrasData.length;
      if (currentCount + selectedCount > 5) {
        alert('Solo puedes subir un máximo de 5 imágenes adicionales.');
        return;
      }

      Array.from(files).forEach((file) => {
        const reader = new FileReader();
        reader.onload = () => {
          if (reader.result) {
            const preview = reader.result as string;
            this.imagenesExtras.push(preview);
            this.imagenesExtrasData.push({ preview, file });
          }
        };
        reader.readAsDataURL(file);
      });
    }
  }

  removeExtraImage(index: number): void {
    const confirmar = confirm('¿Estás segura de querer eliminar esta imagen?');
    if (!confirmar) return;

    const valor = this.imagenesExtras[index];

    if (valor && typeof valor === 'string' && !valor.startsWith('data:')) {
      const nombreArchivo = valor.split('/').pop() || valor;

      this.emprendimientoFormService.borrarImagenAdicional(this.idEmprendimiento, nombreArchivo).subscribe({
        next: () => {
          this.imagenesExtras.splice(index, 1);
          alert('Imagen eliminada correctamente.');
        },
        error: (err) => {
          console.error('Error al eliminar la imagen adicional', err);
          alert('Ocurrió un error al eliminar la imagen adicional. Por favor, intenténtelo de nuevo.');
        }
      });
    } else {
      this.imagenesExtras.splice(index, 1);
      const i = this.imagenesExtrasData.findIndex(img => img.preview === valor);
      if (i > -1) this.imagenesExtrasData.splice(i, 1);
    }
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

    if (this.imagenPrincipalFile) {
      formData.append('Imagen', this.imagenPrincipalFile);
    }

    this.emprendimientoFormService.actualizarEmprendimiento(this.idEmprendimiento, formData).subscribe({
      next: () => {
        const filesToUpload = this.imagenesExtrasData.map(img => img.file);
        if (filesToUpload.length > 0) {
          this.emprendimientoFormService.actualizarMultimedia(this.idEmprendimiento, filesToUpload).subscribe({
            next: () => {
              alert('Emprendimiento e imágenes actualizados correctamente.');
              this.router.navigate(['/mis-emprendimientos']);
            },
            error: (err) => {
              console.error('Error al actualizar las imágenes adicionales', err);
              alert('El emprendimiento y la imagen principal se actualizaron correctamente, pero ocurrió un error al actualizar las imágenes adicionales.');
            }
          });
        } else {
          alert('Emprendimiento actualizado correctamente, pero no se añadieron imágenes adicionales.');
          this.router.navigate(['/mis-emprendimientos']);
        }
      },
      error: (error) => {
        console.error('Error al actualizar el emprendimiento', error);
        alert('Ocurrió un error al actualizar el emprendimiento.');
      }
    });
  }
  
}
