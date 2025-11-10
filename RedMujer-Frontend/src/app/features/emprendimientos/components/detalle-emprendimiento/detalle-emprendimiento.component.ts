import { Component, OnInit } from '@angular/core';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { EmprendimientoService } from '../../services/emprendimiento.service';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

export interface Emprendimiento {
  id_Emprendimiento: number;
  nombre: string;
  descripcion: string;
  imagen?: string;
  categorias?: any[];
  categoriasTexto?: string;
  region?: string;
  comuna?: string;
  modalidad?: string;
  horario_Atencion?: string;
  id_Comuna?: number;
  id_Region?: number;
  numero?: string;
  calle?: string;
  plataformas?: Plataforma[];
  contactos?: Contacto[];
  videoUrl?: SafeResourceUrl;
}

export interface Plataforma {
  ruta: string;
  descripcion: string;
  tipo_plataforma: string;
  icon?: string;
}

export interface Contacto {
  id_Contacto: number;
  valor: string;
  tipo_Contacto: 'telefono' | 'correo';
  vigencia: boolean;
}

@Component({
  selector: 'app-detalle-emprendimiento',
  imports: [MATERIAL_IMPORTS, CommonModule],
  templateUrl: './detalle-emprendimiento.component.html',
  styleUrl: './detalle-emprendimiento.component.scss'
})
export class DetalleEmprendimientoComponent implements OnInit {
  idEmprendimiento!: number;
  emprendimiento: Emprendimiento = {
    id_Emprendimiento: 0,
    nombre: '',
    descripcion: '',
    imagen: '',
    categorias: [],
    categoriasTexto: '',
    region: '',
    comuna: '',
    modalidad: '',
    horario_Atencion: '',
    id_Comuna: 0,
    id_Region: 0,
    numero: '',
    calle: '',
    plataformas: [],
    contactos: [],
    videoUrl: ''
  };

  imagenesExtras: string[] = [];
  loading: boolean = true;
  modalAbierto: boolean = false;
  imagenSeleccionada: string = '';
  indiceImagenActual: number = 0;

  constructor(
    private emprendimientoService: EmprendimientoService,
    private router: Router,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const fullParam = this.route.snapshot.paramMap.get('slug')!;
      const id = fullParam.split('-').pop();
      this.idEmprendimiento = Number(id);
      this.getEmprendimiento();
      this.cargarMultimedias();
    });
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

  getEmprendimiento(): void {
    this.loading = true;

    this.emprendimientoService.getById(this.idEmprendimiento).subscribe({
      next: (data: Emprendimiento) => {
        this.emprendimiento = data;
        this.asignarValores();
      },
      error: (err) => {
        console.error('Error al cargar emprendimiento:', err);
        this.loading = false;
      }
    });
  }

  private asignarValores(): void {
    const ubicacion$ = this.emprendimientoService.getUbicacionDeEmprendimiento(this.idEmprendimiento).pipe(
      catchError(err => {
        console.error(`Error al obtener ubicación del emprendimiento con id ${this.idEmprendimiento}:`, err);
        return of({
          region: '',
          comuna: '',
          id_Comuna: null,
          id_Region: null,
          calle: '',
          numero: '',
        });
      })
    );

    const categorias$ = this.emprendimientoService.getCategoriasByEmprendimiento(this.idEmprendimiento).pipe(
      catchError(err => {
        console.error(`Error al obtener categorías del emprendimiento con id ${this.idEmprendimiento}:`, err);
        return of([]);
      })
    );

    const plataformas$ = this.emprendimientoService.getPlataformasByEmprendimiento(this.idEmprendimiento).pipe(
      catchError(err => {
        console.error(`Error al obtener plataformas del emprendimiento con id ${this.idEmprendimiento}:`, err);
        return of([]);
      })
    );

    const contactos$ = this.emprendimientoService.getContactosByEmprendimiento(this.idEmprendimiento).pipe(
      catchError(err => {
        console.error(`Error al obtener contactos del emprendimiento con id ${this.idEmprendimiento}:`, err);
        return of([]);
      })
    );

    forkJoin({
      ubicacion: ubicacion$,
      categorias: categorias$,
      plataformas: plataformas$,
      contactos: contactos$
    }).subscribe({
      next: (result) => {
        this.emprendimiento.region = result.ubicacion.region;
        this.emprendimiento.comuna = result.ubicacion.comuna;
        this.emprendimiento.id_Comuna = result.ubicacion.id_Comuna;
        this.emprendimiento.id_Region = result.ubicacion.id_Region;
        this.emprendimiento.calle = result.ubicacion.calle;
        this.emprendimiento.numero = result.ubicacion.numero;

        this.emprendimiento.categorias = result.categorias;
        this.emprendimiento.categoriasTexto = result.categorias.map((cat: any) => cat.nombre).join(', ');

        this.emprendimiento.plataformas = result.plataformas.map(plataforma => ({
          ...plataforma,
          icon: this.getIconForPlataforma(plataforma)
        }));

        this.emprendimiento.contactos = result.contactos;

        if (this.emprendimiento.imagen) {
          this.emprendimiento.imagen = `${this.emprendimientoService['apiUrl'].replace('/api', '')}/media/${this.emprendimiento.imagen}`;
        }

        if (this.emprendimiento.videoUrl) {
          const videoUrlString = this.emprendimiento.videoUrl as string;
          const videoId = this.extractYoutubeVideoId(videoUrlString);
          if (videoId) {
            const embedUrl = `https://www.youtube.com/embed/${videoId}`;
            this.emprendimiento.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl);
          }
        }

        this.loading = false;
      },
      error: (err) => {
        console.error('Error al cargar datos adicionales del emprendimiento:', err);

        if (this.emprendimiento.imagen) {
          this.emprendimiento.imagen = `${this.emprendimientoService['apiUrl'].replace('/api', '')}/media/${this.emprendimiento.imagen}`;
        }

        this.loading = false;
      }
    });
  }

  private getIconForPlataforma(plataforma: Plataforma): string {
    const tipo = plataforma.tipo_plataforma?.toLowerCase();
    const descripcion = plataforma.descripcion?.toLowerCase();

    switch (tipo) {
      case 'red_social':
        if (descripcion?.includes('instagram')) {
          return 'camera_alt';
        } else if (descripcion?.includes('facebook')) {
          return 'facebook';
        } else if (descripcion?.includes('x') || descripcion?.includes('twitter')) {
          return 'alternate_email';
        } else if (descripcion?.includes('youtube')) {
          return 'play_circle_filled';
        } else if (descripcion?.includes('linkedin')) {
          return 'business_center';
        } else if (descripcion?.includes('tiktok')) {
          return 'music_note';
        } else if (descripcion?.includes('whatsapp')) {
          return 'message';
        } else {
          return 'share';
        }

      case 'sitio_web':
        return 'language';

      case 'mercado_online':
        return 'shopping_cart';

      case 'aplicacion_movil':
        return 'smartphone';

      case 'otro':
      default:
        return 'link';
    }
  }

  private cargarMultimedias(): void {
    this.emprendimientoService.obtenerMultimediaPorId(this.idEmprendimiento).subscribe({
      next: (imagenes) => {
        this.imagenesExtras = imagenes.map(imagen => {
          if (imagen.startsWith('http://') || imagen.startsWith('https://')) {
            return imagen;
          }
          return `${this.emprendimientoService['apiUrl'].replace('/api', '')}/media/${imagen}`;
        });
      },
      error: (err) => {
        console.error('Error al cargar las imágenes secundarias', err);
      }
    });
  }

  onImageError(event: any): void {
    event.target.src = 'assets/images/emprendimiento-placeholder.png';
  }

  abrirModal(imagen: string, indice: number): void {
    this.imagenSeleccionada = imagen;
    this.indiceImagenActual = indice;
    this.modalAbierto = true;
    document.body.style.overflow = 'hidden';
  }

  cerrarModal(): void {
    this.modalAbierto = false;
    document.body.style.overflow = 'auto';
  }

  siguiente(): void {
    this.indiceImagenActual = (this.indiceImagenActual + 1) % this.imagenesExtras.length;
    this.imagenSeleccionada = this.imagenesExtras[this.indiceImagenActual];
  }

  anterior(): void {
    this.indiceImagenActual = (this.indiceImagenActual - 1 + this.imagenesExtras.length) % this.imagenesExtras.length;
    this.imagenSeleccionada = this.imagenesExtras[this.indiceImagenActual];
  }
}