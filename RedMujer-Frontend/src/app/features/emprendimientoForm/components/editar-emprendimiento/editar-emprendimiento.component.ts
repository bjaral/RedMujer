import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormArray } from '@angular/forms';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { EmprendimientoFormService } from '../../services/emprendimiento-form.service';
import { Router, ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-editar-emprendimiento',
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
  categorias: any[] = [];
  contactosOriginales: any[] = [];
  plataformasOriginales: any[] = [];
  categoriasOriginales: number[] = [];
  videoEmbedUrl: SafeResourceUrl | null = null;

  tiposContacto = ['telefono', 'email'];
  tiposPlataforma = [
    { value: 'red_social', label: 'Red Social' },
    { value: 'sitio_web', label: 'Sitio Web' },
    { value: 'mercado_online', label: 'Mercado Online' },
    { value: 'aplicacion_movil', label: 'Aplicación Móvil' },
    { value: 'otro', label: 'Otro' }
  ];

  redesSociales = ['Instagram', 'Facebook', 'LinkedIn', 'Twitter/X', 'TikTok', 'YouTube', 'Pinterest', 'Otra'];

  constructor(
    private fb: FormBuilder, 
    private emprendimientoFormService: EmprendimientoFormService, 
    private router: Router,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer
  ) {
    this.formulario = this.fb.group({
      rut: ['', Validators.required],
      nombre: ['', Validators.required],
      descripcion: [''],
      modalidad: ['', Validators.required],
      horario_Atencion: ['', Validators.required],
      categorias: [[], Validators.required],
      videoUrl: [''],
      contactos: this.fb.array([]),
      plataformas: this.fb.array([])
    });
  }

  ngOnInit(): void {
    this.cargarCategorias();
    this.route.params.subscribe(params => {
      this.idEmprendimiento = +params['id'];
      if (this.idEmprendimiento) {
        this.cargarEmprendimiento(this.idEmprendimiento);
      }
    });
  }

  cargarCategorias(): void {
    this.emprendimientoFormService.obtenerCategorias().subscribe({
      next: (categorias) => {
        this.categorias = categorias;
      },
      error: (err) => {
        console.error('Error al cargar categorías', err);
      }
    });
  }

  get contactos(): FormArray {
    return this.formulario.get('contactos') as FormArray;
  }

  get plataformas(): FormArray {
    return this.formulario.get('plataformas') as FormArray;
  }

  agregarContacto(): void {
    const contactoGroup = this.fb.group({
      id_Contacto: [null],
      valor: [''],
      tipo_Contacto: ['telefono']
    });
    this.contactos.push(contactoGroup);
  }

  eliminarContacto(index: number): void {
    const confirmar = confirm('¿Estás segura de querer eliminar este contacto?');
    if (!confirmar) return;

    const contacto = this.contactos.at(index).value;
    if (contacto.id_Contacto) {
      this.emprendimientoFormService.eliminarContacto(contacto.id_Contacto).subscribe({
        next: () => {
          this.contactos.removeAt(index);
          alert('Contacto eliminado correctamente.');
        },
        error: (err) => {
          console.error('Error al eliminar contacto', err);
          alert('Error al eliminar el contacto.');
        }
      });
    } else {
      if (this.contactos.length > 1) {
        this.contactos.removeAt(index);
      }
    }
  }

  agregarPlataforma(): void {
    const plataformaGroup = this.fb.group({
      id_Plataforma: [null],
      ruta: [''],
      tipo_Plataforma: ['sitio_web'],
      descripcion: ['']
    });
    this.plataformas.push(plataformaGroup);
  }

  eliminarPlataforma(index: number): void {
    const confirmar = confirm('¿Estás segura de querer eliminar esta plataforma?');
    if (!confirmar) return;

    const plataforma = this.plataformas.at(index).value;
    if (plataforma.id_Plataforma) {
      this.emprendimientoFormService.eliminarPlataforma(plataforma.id_Plataforma).subscribe({
        next: () => {
          this.plataformas.removeAt(index);
          alert('Plataforma eliminada correctamente.');
        },
        error: (err) => {
          console.error('Error al eliminar plataforma', err);
          alert('Error al eliminar la plataforma.');
        }
      });
    } else {
      if (this.plataformas.length > 1) {
        this.plataformas.removeAt(index);
      }
    }
  }

  esTipoRedSocial(index: number): boolean {
    const tipo = this.plataformas.at(index).get('tipo_Plataforma')?.value;
    return tipo === 'red_social';
  }

  onVideoUrlChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const url = input.value.trim();
    
    if (!url) {
      this.videoEmbedUrl = null;
      return;
    }

    const videoId = this.extractYoutubeVideoId(url);
    if (videoId) {
      const embedUrl = `https://www.youtube.com/embed/${videoId}`;
      this.videoEmbedUrl = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl);
    } else {
      this.videoEmbedUrl = null;
      alert('Por favor ingresa una URL válida de YouTube.');
    }
  }

  extractYoutubeVideoId(url: string): string | null {
    const patterns = [
      /(?:https?:\/\/)?(?:www\.)?youtube\.com\/watch\?v=([^&]+)/,
      /(?:https?:\/\/)?(?:www\.)?youtu\.be\/([^?]+)/,
      /(?:https?:\/\/)?(?:www\.)?youtube\.com\/embed\/([^?]+)/
    ];

    for (const pattern of patterns) {
      const match = url.match(pattern);
      if (match && match[1]) {
        return match[1];
      }
    }
    return null;
  }

  removeVideo(): void {
    const confirmar = confirm('¿Estás segura de querer eliminar el video?');
    if (!confirmar) return;
    this.formulario.get('videoUrl')?.setValue('');
    this.videoEmbedUrl = null;
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
          videoUrl: emprendimiento.videoUrl || ''
        });

        // Cargar video si existe
        if (emprendimiento.videoUrl) {
          const videoId = this.extractYoutubeVideoId(emprendimiento.videoUrl);
          if (videoId) {
            const embedUrl = `https://www.youtube.com/embed/${videoId}`;
            this.videoEmbedUrl = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl);
          }
        }

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

        this.cargarContactos();
        this.cargarCategoriasSeleccionadas();
        this.cargarPlataformas();
      },
      error: (error) => {
        console.error('Error al cargar el emprendimiento', error);
        alert('Ocurrió un error al cargar el emprendimiento. Por favor, inténtelo de nuevo');
        this.router.navigate(['/mis-emprendimientos']);
      }
    });
  }

  cargarContactos(): void {
    this.emprendimientoFormService.obtenerContactosPorEmprendimiento(this.idEmprendimiento).subscribe({
      next: (contactos) => {
        this.contactosOriginales = contactos;
        this.contactos.clear();
        if (contactos && contactos.length > 0) {
          contactos.forEach(contacto => {
            const contactoGroup = this.fb.group({
              id_Contacto: [contacto.id_Contacto],
              valor: [contacto.valor],
              tipo_Contacto: [contacto.tipo_Contacto]
            });
            this.contactos.push(contactoGroup);
          });
        } else {
          this.agregarContacto();
        }
      },
      error: (err) => {
        console.error('Error al cargar contactos', err);
        this.agregarContacto();
      }
    });
  }

  cargarCategoriasSeleccionadas(): void {
    this.emprendimientoFormService.obtenerCategoriasPorEmprendimiento(this.idEmprendimiento).subscribe({
      next: (categorias) => {
        const categoriasIds = categorias.map(cat => cat.id_Categoria);
        this.categoriasOriginales = categoriasIds;
        this.formulario.patchValue({ categorias: categoriasIds });
      },
      error: (err) => {
        console.error('Error al cargar categorías del emprendimiento', err);
      }
    });
  }

  cargarPlataformas(): void {
    this.emprendimientoFormService.obtenerPlataformasPorEmprendimiento(this.idEmprendimiento).subscribe({
      next: (plataformas) => {
        this.plataformasOriginales = plataformas;
        this.plataformas.clear();
        if (plataformas && plataformas.length > 0) {
          plataformas.forEach(plataforma => {
            const plataformaGroup = this.fb.group({
              id_Plataforma: [plataforma.id_Plataforma],
              ruta: [plataforma.ruta],
              tipo_Plataforma: [plataforma.tipo_Plataforma],
              descripcion: [plataforma.descripcion || '']
            });
            this.plataformas.push(plataformaGroup);
          });
        } else {
          this.agregarPlataforma();
        }
      },
      error: (err) => {
        console.error('Error al cargar plataformas', err);
        this.agregarPlataforma();
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
        alert('Ocurrió un error al eliminar la imagen principal. Por favor, inténtelo de nuevo.');
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
          alert('Ocurrió un error al eliminar la imagen adicional. Por favor, inténtelo de nuevo.');
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
    formData.append('VideoUrl', data.videoUrl);

    if (this.imagenPrincipalFile) {
      formData.append('Imagen', this.imagenPrincipalFile);
    }

    this.emprendimientoFormService.actualizarEmprendimiento(this.idEmprendimiento, formData).subscribe({
      next: () => {
        const filesToUpload = this.imagenesExtrasData.map(img => img.file);
        if (filesToUpload.length > 0) {
          this.emprendimientoFormService.actualizarMultimedia(this.idEmprendimiento, filesToUpload).subscribe({
            next: () => {
              console.log('Multimedia actualizada');
            },
            error: (err) => {
              console.error('Error al actualizar las imágenes adicionales', err);
            }
          });
        }
        
        this.actualizarContactos();
        this.actualizarCategorias();
        this.actualizarPlataformas();
        
        alert('Emprendimiento actualizado exitosamente.');
        this.router.navigate(['/mis-emprendimientos']);
      },
      error: (error) => {
        console.error('Error al actualizar el emprendimiento', error);
        alert('Ocurrió un error al actualizar el emprendimiento.');
      }
    });
  }

  actualizarContactos(): void {
    const contactos = this.formulario.get('contactos')?.value;
    contactos.forEach((contacto: any) => {
      if (contacto.valor) {
        const contactoData = {
          id_Emprendimiento: this.idEmprendimiento,
          valor: contacto.valor,
          tipo_Contacto: contacto.tipo_Contacto,
          vigencia: true
        };

        if (contacto.id_Contacto) {
          this.emprendimientoFormService.actualizarContacto(contacto.id_Contacto, contactoData).subscribe({
            next: () => console.log('Contacto actualizado'),
            error: (err) => console.error('Error al actualizar contacto', err)
          });
        } else {
          this.emprendimientoFormService.crearContacto(contactoData).subscribe({
            next: () => console.log('Contacto creado'),
            error: (err) => console.error('Error al crear contacto', err)
          });
        }
      }
    });
  }

  actualizarCategorias(): void {
    const categoriasSeleccionadas = this.formulario.get('categorias')?.value || [];
    
    // Eliminar categorías que ya no están seleccionadas
    this.categoriasOriginales.forEach(idCategoria => {
      if (!categoriasSeleccionadas.includes(idCategoria)) {
        this.emprendimientoFormService.eliminarEmprendimientoCategoria(this.idEmprendimiento, idCategoria).subscribe({
          next: () => console.log('Categoría eliminada'),
          error: (err) => console.error('Error al eliminar categoría', err)
        });
      }
    });

    // Agregar nuevas categorías
    categoriasSeleccionadas.forEach((idCategoria: number) => {
      if (!this.categoriasOriginales.includes(idCategoria)) {
        const empCat = {
          id_Categoria: idCategoria,
          id_Emprendimiento: this.idEmprendimiento
        };
        this.emprendimientoFormService.crearEmprendimientoCategoria(empCat).subscribe({
          next: () => console.log('Categoría asociada'),
          error: (err) => console.error('Error al asociar categoría', err)
        });
      }
    });
  }

  actualizarPlataformas(): void {
    const plataformas = this.formulario.get('plataformas')?.value;
    plataformas.forEach((plataforma: any) => {
      if (plataforma.ruta) {
        const plataformaData = {
          id_Emprendimiento: this.idEmprendimiento,
          ruta: plataforma.ruta,
          tipo_Plataforma: plataforma.tipo_Plataforma,
          descripcion: plataforma.descripcion || '',
          vigencia: true
        };

        if (plataforma.id_Plataforma) {
          this.emprendimientoFormService.actualizarPlataforma(plataforma.id_Plataforma, plataformaData).subscribe({
            next: () => console.log('Plataforma actualizada'),
            error: (err) => console.error('Error al actualizar plataforma', err)
          });
        } else {
          this.emprendimientoFormService.crearPlataforma(plataformaData).subscribe({
            next: () => console.log('Plataforma creada'),
            error: (err) => console.error('Error al crear plataforma', err)
          });
        }
      }
    });
  }

  get maximoImagenesExtrasAlcanzado(): boolean {
    return this.imagenesExtras.length >= 5;
  }
  
}