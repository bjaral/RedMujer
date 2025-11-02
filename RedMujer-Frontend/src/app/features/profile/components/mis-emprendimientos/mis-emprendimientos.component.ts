import { Component, OnInit, Input } from '@angular/core';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { ProfileService } from '../../services/profile.service';
import { TokenService } from '../../../../core/services/token.service';
import { PersonaService } from '../../services/persona.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-mis-emprendimientos',
  standalone: true,
  imports: [MATERIAL_IMPORTS, RouterModule, CommonModule],
  templateUrl: './mis-emprendimientos.component.html',
  styleUrl: './mis-emprendimientos.component.scss'
})
export class MisEmprendimientosComponent implements OnInit {

  emprendimientos: any[] = [];
  idUsuario: number = 0;
  idPersona: number = 0;

  constructor(private profileService: ProfileService, private tokenService: TokenService, private personaService: PersonaService, private router: Router) {}

  ngOnInit(): void {
    this.cargarEmprendimientos();
  }

  cargarEmprendimientos(): void {
    this.idUsuario = this.tokenService.getNameIdentifier() || 0;
    this.personaService.getIdPersona(this.idUsuario).subscribe({
      next: (id) => {
        this.idPersona = id;
        console.log('ID de la persona:', this.idPersona);

        this.profileService.obtenerEmprendimientosPorPersona(this.idPersona).subscribe({
          next: (data) => {
            this.emprendimientos = data.map((emprendimiento) => {
              if (emprendimiento.imagen) {
                emprendimiento.imagen = `http://localhost:5145/media/${emprendimiento.imagen}`;
              }
              return emprendimiento;
            });
          },
          error: (err) => {
            console.error('Error al cargar emprendimientos:', err);
          }
        });
      },
      error: (err) => {
        console.error('Error al obtener el ID de persona:', err);
      }
    });
  }

  toEditar(idEmprendimiento: number): void {
    this.router.navigate(['/editar-emprendimiento/', idEmprendimiento]);
  }

  toEmprendimiento(idEmprendimiento: number): void {
    this.router.navigate(['/emprendimientos/', idEmprendimiento]);
  }

  eliminarEmprendimiento(idEmprendimiento: number): void {
    const confirmar = confirm('¿Estás segura de que deseas eliminar este emprendimiento? Esta acción no se puede deshacer.');
    if (!confirmar) return;

    this.profileService.borrarEmprendimientoPorId(idEmprendimiento).subscribe({
      next: () => {
        this.emprendimientos = this.emprendimientos.filter(e => e.id_Emprendimiento !== idEmprendimiento);
        alert('Emprendimiento eliminado correctamente.');
      },
      error: (err) => {
        console.error('Error al eliminar el emprendimiento', err);
        alert('Ocurrió un error al eliminar el emprendimiento. Por favor, inténtalo de nuevo más tarde.');
      }
    });
  }

  onImageError(event: any): void {
    event.target.src = 'assets/images/emprendimiento-placeholder.png';
  }

}
