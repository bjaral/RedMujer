import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { EmprendimientoService } from '../../services/emprendimiento.service';
import { FormsModule } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { UbicacionService } from '../../services/ubicacion.service';
import { Router } from '@angular/router';
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
  fecha_inicio?: Date;
  id_Comuna?: number;
  id_Region?: number;
}

@Component({
  selector: 'app-emprendimientos',
  standalone: true,
  imports: [...MATERIAL_IMPORTS, CommonModule, FormsModule],
  templateUrl: './emprendimientos.component.html',
  styleUrl: './emprendimientos.component.scss'
})
export class EmprendimientosComponent implements OnInit {
  emprendimientos: Emprendimiento[] = [];
  emprendimientosFiltrados: Emprendimiento[] = [];
  emprendimientosPaginados: Emprendimiento[] = [];
  modos: any[] = [];
  comunas: any[] = [];
  comunasFiltradas: any[] = [];
  regiones: any[] = [];
  categorias: any[] = [];

  loading = true;
  searchTerm = '';
  selectedRegion = '';
  selectedComuna = '';
  selectedModo = '';
  selectedCategoria = '';
  sortOrder = 'default';

  itemsPorPagina = 20;
  paginaActual = 0;
  totalItems = 0;

  constructor(
    private router: Router,
    private emprendimientoService: EmprendimientoService,
    private ubicacionService: UbicacionService
  ) { }

  ngOnInit(): void {
    this.getEmprendimientos();
    this.loadFilters();
  }

  getEmprendimientos(): void {
    this.loading = true;

    this.emprendimientoService.getAll().subscribe({
      next: (data: Emprendimiento[]) => {
        this.asignarUbicacionesAEmprendimientos(data);
      },
      error: (err) => {
        console.error('Error al cargar emprendimientos:', err);
        this.loading = false;
      }
    });
  }

  private asignarUbicacionesAEmprendimientos(emprendimientos: Emprendimiento[]): void {
    const emprendimientosConUbicacion$ = emprendimientos.map((emp: Emprendimiento) => {
      return this.emprendimientoService.getUbicacionDeEmprendimiento(Number(emp.id_Emprendimiento)).pipe(
        map((ubicacion: any) => {
          emp.region = ubicacion.region;
          emp.comuna = ubicacion.comuna;
          emp.id_Comuna = ubicacion.id_Comuna;
          emp.id_Region = ubicacion.id_Region;

          if (emp.imagen) {
            emp.imagen = `http://localhost:5145/media/${emp.imagen}`;
          }

          return emp;
        }),
        catchError(err => {
          console.error(`Error al obtener ubicación del emprendimiento con id ${emp.id_Emprendimiento}:`, err);

          if (emp.imagen) {
            emp.imagen = `http://localhost:5145/media/${emp.imagen}`;
          }

          return of(emp);
        })
      );
    });

    forkJoin(emprendimientosConUbicacion$).subscribe({
      next: (emprendimientosConUbicacion: Emprendimiento[]) => {
        this.emprendimientos = emprendimientosConUbicacion;
        this.emprendimientosFiltrados = [...this.emprendimientos];
        this.totalItems = this.emprendimientos.length;
        this.extractCategorias();
        this.aplicarOrden();
        this.loading = false;
      },
      error: (err: any) => {
        console.error('Error al cargar ubicaciones de los emprendimientos:', err);
        this.loading = false;
      }
    });
  }

  private extractCategorias(): void {
    const categoriasSet = new Set();
    this.emprendimientos.forEach((emp: Emprendimiento) => {
      if (emp.categorias && emp.categorias.length > 0) {
        emp.categorias.forEach((cat: any) => {
          categoriasSet.add(JSON.stringify({ id: cat.id, nombre: cat.nombre }));
        });
      }
    });
    this.categorias = Array.from(categoriasSet).map(cat => JSON.parse(cat as string));
  }

  loadFilters(): void {
    this.ubicacionService.regiones().subscribe({
      next: (regiones) => {
        this.regiones = regiones;
      },
      error: (err) => {
        console.error('Error al cargar regiones', err);
      }
    });

    this.modos = [
      { id: 'Presencial', nombre: 'Presencial' },
      { id: 'Online', nombre: 'Online' },
      { id: 'PresencialYOnline', nombre: 'Híbrida' }
    ];

    this.ubicacionService.comunas().subscribe({
      next: (comunas) => {
        this.comunas = comunas;
        this.comunasFiltradas = [];
      },
      error: (err) => {
        console.error('Error al cargar comunas', err);
      }
    });
  }

  onRegionChange(): void {
    if (this.selectedRegion && this.selectedRegion !== 'todas') {
      const regionId = Number(this.selectedRegion);

      this.ubicacionService.comunasPorRegion(regionId).subscribe({
        next: (comunas) => {
          this.comunasFiltradas = comunas;
          this.selectedComuna = 'todas';
          this.filtrarEmprendimientos();
        },
        error: (err) => {
          console.error('Error al cargar comunas por región', err);
          this.comunasFiltradas = [];
          this.selectedComuna = 'todas';
          this.filtrarEmprendimientos();
        }
      });
    } else {
      this.comunasFiltradas = [];
      this.selectedComuna = 'todas';
      this.filtrarEmprendimientos();
    }
  }

  filtrarEmprendimientos(): void {
    let resultados: Emprendimiento[] = [...this.emprendimientos];

    if (this.searchTerm.trim()) {
      const termino = this.searchTerm.toLowerCase().trim();
      resultados = resultados.filter((emp: Emprendimiento) =>
        emp.nombre?.toLowerCase().includes(termino) ||
        emp.descripcion?.toLowerCase().includes(termino) ||
        emp.categoriasTexto?.toLowerCase().includes(termino)
      );
    }

    if (this.selectedRegion && this.selectedRegion !== 'todas') {
      const regionId = Number(this.selectedRegion);
      resultados = resultados.filter((emp: Emprendimiento) => 
        emp.id_Region === regionId
      );
    }

    if (this.selectedComuna && this.selectedComuna !== 'todas') {
      const comunaId = Number(this.selectedComuna);
      resultados = resultados.filter((emp: Emprendimiento) => 
        emp.id_Comuna === comunaId
      );
    }

    if (this.selectedModo && this.selectedModo !== 'todas') {
      resultados = resultados.filter((emp: Emprendimiento) =>
        emp.modalidad === this.selectedModo
      );
    }

    if (this.selectedCategoria && this.selectedCategoria !== 'todas') {
      resultados = resultados.filter((emp: Emprendimiento) =>
        emp.categorias && emp.categorias.some((cat: any) => cat.id.toString() === this.selectedCategoria)
      );
    }

    this.emprendimientosFiltrados = resultados;
    this.totalItems = resultados.length;
    this.paginaActual = 0;
    this.aplicarOrden();
  }

  buscarEmprendimientos(): void {
    this.filtrarEmprendimientos();
  }

  aplicarOrden(): void {
    let resultados: Emprendimiento[] = [...this.emprendimientosFiltrados];

    switch (this.sortOrder) {
      case 'nombre-asc':
        resultados.sort((a: Emprendimiento, b: Emprendimiento) => a.nombre?.localeCompare(b.nombre || ''));
        break;
      case 'nombre-desc':
        resultados.sort((a: Emprendimiento, b: Emprendimiento) => b.nombre?.localeCompare(a.nombre || ''));
        break;
      case 'fecha-reciente':
        resultados.sort((a: Emprendimiento, b: Emprendimiento) => new Date(b.fecha_inicio || 0).getTime() -
          new Date(a.fecha_inicio || 0).getTime());
        break;
      case 'fecha-antigua':
        resultados.sort((a: Emprendimiento, b: Emprendimiento) => new Date(a.fecha_inicio || 0).getTime() -
          new Date(b.fecha_inicio || 0).getTime());
        break;
      case '10':
        this.itemsPorPagina = 10;
        break;
      case '20':
        this.itemsPorPagina = 20;
        break;
      case '30':
        this.itemsPorPagina = 30;
        break;
      default:
        break;
    }

    this.emprendimientosFiltrados = resultados;
    this.paginarResultados();
  }

  onPageChange(event: PageEvent): void {
    this.paginaActual = event.pageIndex;
    this.itemsPorPagina = event.pageSize;
    this.paginarResultados();
  }

  paginarResultados(): void {
    const inicio = this.paginaActual * this.itemsPorPagina;
    const fin = inicio + this.itemsPorPagina;
    this.emprendimientosPaginados = this.emprendimientosFiltrados.slice(inicio, fin);
  }

  limpiarFiltros(): void {
    this.searchTerm = '';
    this.selectedRegion = 'todas';
    this.selectedComuna = 'todas';
    this.selectedModo = 'todas';
    this.selectedCategoria = 'todas';
    this.sortOrder = 'default';
    this.paginaActual = 0;
    this.comunasFiltradas = [];
    this.emprendimientosFiltrados = [...this.emprendimientos];
    this.totalItems = this.emprendimientos.length;
    this.aplicarOrden();
  }

  verDetalles(emp: Emprendimiento): void {
    this.router.navigate(['/emprendimientos', emp.id_Emprendimiento]);
  }

  get contadorTexto(): string {
    if (this.emprendimientosFiltrados.length === 0) {
      return 'No se encontraron emprendimientos';
    }

    const inicio = this.paginaActual * this.itemsPorPagina + 1;
    const fin = Math.min((this.paginaActual + 1) * this.itemsPorPagina, this.totalItems);

    if (this.totalItems <= this.itemsPorPagina) {
      return `Mostrando ${this.totalItems} emprendimiento${this.totalItems !== 1 ? 's' : ''}`;
    }

    return `Mostrando ${inicio}-${fin} de ${this.totalItems} emprendimientos`;
  }

  onImageError(event: any): void {
    event.target.src = 'assets/images/emprendimiento-placeholder.png';
  }
}