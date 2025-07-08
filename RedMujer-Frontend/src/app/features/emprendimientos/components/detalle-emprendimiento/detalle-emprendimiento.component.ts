import { Component, OnInit } from '@angular/core';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { EmprendimientoService } from '../../services/emprendimiento.service';
import { forkJoin, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

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
  id_Comuna?: number;
  id_Region?: number;
  numero?: string;
  calle?: string;
  plataformas?: Plataforma[],
}

export interface Plataforma {
  ruta: string;
  descripcion: string;
  tipo_plataforma: string;
  icon?: string; // Nuevo atributo para el icono
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
    id_Comuna: 0,
    id_Region: 0,
    numero: '',
    calle: '',
    plataformas: [],
  };

  imagenesExtras: string[] = [];
  loading: boolean = true;

  constructor(
    private emprendimientoService: EmprendimientoService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.idEmprendimiento = +params['id'];
      this.getEmprendimiento();
      this.cargarMultimedias();
    });
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
    )

    forkJoin({
      ubicacion: ubicacion$,
      categorias: categorias$,
      plataformas: plataformas$
    }).subscribe({
      next: (result) => {
        // Asignar ubicación
        this.emprendimiento.region = result.ubicacion.region;
        this.emprendimiento.comuna = result.ubicacion.comuna;
        this.emprendimiento.id_Comuna = result.ubicacion.id_Comuna;
        this.emprendimiento.id_Region = result.ubicacion.id_Region;
        this.emprendimiento.calle = result.ubicacion.calle;
        this.emprendimiento.numero = result.ubicacion.numero;

        // Asignar categorías
        this.emprendimiento.categorias = result.categorias;
        this.emprendimiento.categoriasTexto = result.categorias.map((cat: any) => cat.nombre).join(', ');

        // Asignar plataformas con iconos
        this.emprendimiento.plataformas = result.plataformas.map(plataforma => ({
          ...plataforma,
          icon: this.getIconForPlataforma(plataforma)
        }));

        // Procesar imagen
        if (this.emprendimiento.imagen) {
          this.emprendimiento.imagen = `http://localhost:5145/media/${this.emprendimiento.imagen}`;
        }

        this.loading = false;
      },
      error: (err) => {
        console.error('Error al cargar datos adicionales del emprendimiento:', err);

        if (this.emprendimiento.imagen) {
          this.emprendimiento.imagen = `http://localhost:5145/media/${this.emprendimiento.imagen}`;
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
        return 'language'; // Icono de mundo/web

      case 'mercado_online':
        return 'shopping_cart'; // Icono de carrito de compras

      case 'aplicacion_movil':
        return 'smartphone'; // Icono de móvil

      case 'otro':
      default:
        return 'link'; // Icono genérico de enlace
    }
  }

  private cargarMultimedias(): void {
    this.emprendimientoService.obtenerMultimediaPorId(this.idEmprendimiento).subscribe({
      next: (imagenes) => {
        console.log('Imágenes recibidas del servicio:', imagenes);

        // Verificar si las imágenes ya tienen la URL completa
        this.imagenesExtras = imagenes.map(imagen => {
          console.log('Imagen original:', imagen);

          // Si la imagen ya tiene http:// o https://, no agregar el prefijo
          if (imagen.startsWith('http://') || imagen.startsWith('https://')) {
            console.log('URL completa:', imagen);
            return imagen;
          }

          // Si no, construir la URL completa
          const urlCompleta = `http://localhost:5145/media/${imagen}`;
          console.log('URL construida:', urlCompleta);
          return urlCompleta;
        });

        console.log('URLs finales:', this.imagenesExtras);
      },
      error: (err) => {
        console.error('Error al cargar las imágenes secundarias', err);
      }
    });
  }

  onImageError(event: any): void {
    event.target.src = 'assets/images/emprendimiento-placeholder.png';
  }
}