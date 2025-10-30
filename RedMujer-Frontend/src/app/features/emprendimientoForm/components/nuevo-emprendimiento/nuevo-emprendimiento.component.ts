import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { EmprendimientoFormService } from '../../services/emprendimiento-form.service';
import { UbicacionService } from '../../services/ubicacion.service';
import { TokenService } from '../../../../core/services/token.service';
import { PersonaService } from '../../../profile/services/persona.service';
import { Router } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-nuevo-emprendimiento',
  standalone: true,
  imports: [MATERIAL_IMPORTS, ReactiveFormsModule, CommonModule],
  templateUrl: './nuevo-emprendimiento.component.html',
  styleUrl: './nuevo-emprendimiento.component.scss'
})

export class NuevoEmprendimientoComponent implements OnInit {

  formulario: FormGroup;
  imagenSeleccionada: string | ArrayBuffer | null = null;
  imagenesExtras: string[] = [];
  imagenPrincipalFile: File | null = null;
  imagenesExtrasFile: File[] = [];
  idUsuario: number = 0;
  idPersona: number = 0;
  idEmprendimiento: number = 0;
  idUbicacion: number = 0;
  categorias: any[] = [];
  regiones: any[] = [];
  comunas: any[] = [];
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
    private ubicacionService: UbicacionService,
    private personaService: PersonaService,
    private tokenService: TokenService,
    private router: Router,
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
      plataformas: this.fb.array([]),
      ubicacion: this.fb.group({
        region: [null, Validators.required],
        comuna: [null, Validators.required],
        calle: ['', Validators.required],
        numero: ['', Validators.required],
        referencia: ['']
      })
    });
  }

  ngOnInit(): void {
    this.cargarCategorias();
    this.cargarRegiones();
    this.agregarContacto();
    this.agregarPlataforma();
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

  cargarRegiones(): void {
    this.ubicacionService.regiones().subscribe({
      next: (regiones) => {
        this.regiones = regiones;
      },
      error: (err) => {
        console.error('Error al cargar regiones', err);
      }
    });
  }

  onRegionChange(): void {
    const regionObj = this.formulario.get('ubicacion.region')?.value;
    if (regionObj && regionObj.id_Region) {
      this.ubicacionService.comunasPorRegion(regionObj.id_Region).subscribe({
        next: (comunas) => {
          this.comunas = comunas;
          this.formulario.get('ubicacion.comuna')?.setValue(null);
        },
        error: (err) => {
          console.error('Error al cargar comunas', err);
        }
      });
    } else {
      this.comunas = [];
      this.formulario.get('ubicacion.comuna')?.setValue(null);
    }
  }

  get contactos(): FormArray {
    return this.formulario.get('contactos') as FormArray;
  }

  get plataformas(): FormArray {
    return this.formulario.get('plataformas') as FormArray;
  }

  agregarContacto(): void {
    const contactoGroup = this.fb.group({
      valor: [''],
      tipo_Contacto: ['telefono']
    });
    this.contactos.push(contactoGroup);
  }

  eliminarContacto(index: number): void {
    if (this.contactos.length > 1) {
      this.contactos.removeAt(index);
    }
  }

  agregarPlataforma(): void {
    const plataformaGroup = this.fb.group({
      ruta: [''],
      tipo_Plataforma: ['sitio_web'],
      descripcion: ['']
    });
    this.plataformas.push(plataformaGroup);
  }

  eliminarPlataforma(index: number): void {
    if (this.plataformas.length > 1) {
      this.plataformas.removeAt(index);
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

    this.formulario.get('rut')?.setValue(formattedRut, { emitEvent: false });
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
    const confirmar = confirm('¿Estás segura de querer eliminar esta imagen?');
    if (!confirmar) return;
    this.imagenesExtras.splice(index, 1);
    this.imagenesExtrasFile.splice(index, 1);
  }

  onSubmit(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      alert('Por favor completa todos los campos obligatorios.');
      return;
    }

    // Primero crear la ubicación
    const ubicacionData = this.formulario.get('ubicacion')?.value;
    const regionObj = ubicacionData.region;
    const comunaObj = this.comunas.find(c => c.nombre === ubicacionData.comuna);

    const ubicacion = {
      id_Region: Number(regionObj.id_Region),
      id_Comuna: Number(comunaObj.id_Comuna),
      calle: ubicacionData.calle,
      numero: String(ubicacionData.numero),
      referencia: ubicacionData.referencia || '',
      vigencia: true
    };

    this.emprendimientoFormService.crearUbicacion(ubicacion).subscribe({
      next: (ubicacionRes) => {
        this.idUbicacion = ubicacionRes.id_Ubicacion;
        this.crearEmprendimiento();
      },
      error: (err) => {
        console.error('Error al crear ubicación', err);
        alert('Ocurrió un error al crear la ubicación.');
      }
    });
  }

  private crearEmprendimiento(): void {
    const formData = new FormData();
    const data = this.formulario.value;

    formData.append('RUT', data.rut);
    formData.append('Nombre', data.nombre);
    formData.append('Descripcion', data.descripcion || '');
    formData.append('Modalidad', data.modalidad);
    formData.append('Horario_Atencion', data.horario_Atencion);
    formData.append('Vigencia', 'true');
    formData.append('VideoUrl', data.videoUrl || '');
    formData.append('Id_Ubicacion', this.idUbicacion.toString());
    
    if (this.imagenPrincipalFile) {
      formData.append('Imagen', this.imagenPrincipalFile);
    }

    this.emprendimientoFormService.crearEmprendimiento(formData).subscribe({
      next: (response) => {
        const idEmprendimiento = response?.id_Emprendimiento;
        if (!idEmprendimiento) {
          alert('Error: no se recibió el ID del emprendimiento creado.');
          return;
        }

        this.idEmprendimiento = idEmprendimiento;

        this.idUsuario = this.tokenService.getNameIdentifier() || 0;
        this.personaService.getIdPersona(this.idUsuario).subscribe({
          next: (idPersona) => {
            this.idPersona = idPersona;
            console.log('ID de la persona:', this.idPersona);

            this.personaService.postEmprendimientoToPersona(this.idPersona, this.idEmprendimiento).subscribe({
              next: () => {
                console.log('Emprendimiento asociado a la persona correctamente.');
                this.guardarContactos();
                this.guardarCategorias();
                this.guardarPlataformas();
              },
              error: (err) => {
                console.error('Error al asociar el emprendimiento a la persona:', err);
              }
            });
          },
          error: (err) => {
            console.error('Error al obtener el ID de la persona:', err);
          }
        });

        if (this.imagenesExtrasFile.length > 0) {
          this.emprendimientoFormService.subirMultimedia(idEmprendimiento, this.imagenesExtrasFile).subscribe({
            next: () => {
              console.log('Multimedia subida exitosamente.');
            },
            error: (err) => {
              console.error('Error al subir multimedia', err);
            }
          });
        }
      },
      error: (err) => {
        console.error('Error al crear el emprendimiento', err);
        alert('Ocurrió un error al crear el emprendimiento.');
      }
    });
  }

  guardarContactos(): void {
    const contactos = this.formulario.get('contactos')?.value;
    contactos.forEach((contacto: any) => {
      if (contacto.valor) {
        const contactoData = {
          id_Emprendimiento: this.idEmprendimiento,
          valor: contacto.valor,
          tipo_Contacto: contacto.tipo_Contacto,
          vigencia: true
        };
        this.emprendimientoFormService.crearContacto(contactoData).subscribe({
          next: () => console.log('Contacto guardado'),
          error: (err) => console.error('Error al guardar contacto', err)
        });
      }
    });
  }

  guardarCategorias(): void {
    const categorias = this.formulario.get('categorias')?.value;
    categorias.forEach((idCategoria: number) => {
      const empCat = {
        id_Categoria: idCategoria,
        id_Emprendimiento: this.idEmprendimiento
      };
      this.emprendimientoFormService.crearEmprendimientoCategoria(empCat).subscribe({
        next: () => console.log('Categoría asociada'),
        error: (err) => console.error('Error al asociar categoría', err)
      });
    });
  }

  guardarPlataformas(): void {
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
        this.emprendimientoFormService.crearPlataforma(plataformaData).subscribe({
          next: () => {
            console.log('Plataforma guardada');
            alert('Emprendimiento creado exitosamente.');
            this.limpiarFormulario();
            this.router.navigate(['/mis-emprendimientos']);
          },
          error: (err) => console.error('Error al guardar plataforma', err)
        });
      }
    });
  }

  private limpiarFormulario(): void {
    this.formulario.reset();
    this.imagenSeleccionada = null;
    this.imagenPrincipalFile = null;
    this.imagenesExtras = [];
    this.imagenesExtrasFile = [];
    this.videoEmbedUrl = null;
    this.contactos.clear();
    this.plataformas.clear();
    this.comunas = [];
    this.agregarContacto();
    this.agregarPlataforma();
  }

  get maximoImagenesExtrasAlcanzado(): boolean {
    return this.imagenesExtras.length >= 5;
  }

  get imagenPrincipalDeshabilitada(): boolean {
    return this.imagenPrincipalFile !== null;
  }
  
}