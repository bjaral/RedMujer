import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OnInit } from '@angular/core';
import { HomeService } from '../../services/home.service';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { Router } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

export interface Emprendimiento {
  id_Emprendimiento: number;
  nombre: string;
  descripcion: string;
  imagen?: string;
  categorias?: any[];
  categoriasTexto?: string;
}

@Component({
  selector: 'app-home',
  imports: [CommonModule, ...MATERIAL_IMPORTS],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})

export class HomeComponent implements OnInit {
  random: Emprendimiento[] = [];

  constructor(private homeService: HomeService, private router: Router) { }

  ngOnInit(): void {
    this.getRandom();
  }

  getRandom(): void {
    this.homeService.getRandom().subscribe({
      next: (data: Emprendimiento[]) => {
        this.random = data.map((emp: Emprendimiento) => {
          if (emp.imagen) {
            emp.imagen = encodeURI(`http://localhost:5145/media/${emp.imagen}`);
          }
          return emp;
        });
      },
      error: (err) => {
        console.error('Error al cargar emprendimientos:', err);
      }
    });
  }

  toRegistro() {
    this.router.navigate(['/registro'])
  }

  toEmprendimientos() {
    this.router.navigate(['/emprendimientos']);
  }

  verDetalles(emp: Emprendimiento): void {
    this.router.navigate(['/emprendimientos', emp.id_Emprendimiento]);
  }

  onImageError(event: any): void {
    event.target.src = 'assets/images/emprendimiento-placeholder.png';
  }

}