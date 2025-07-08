import { Component, OnInit } from '@angular/core';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { ProfileService } from '../../services/profile.service';
import { TokenService } from '../../../../core/services/token.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-mis-emprendimientos',
  imports: [MATERIAL_IMPORTS, RouterModule, CommonModule],
  templateUrl: './mis-emprendimientos.component.html',
  styleUrl: './mis-emprendimientos.component.scss'
})

export class MisEmprendimientosComponent implements OnInit {

  emprendimientos: any[] = [];
  idPersona: number = 1;

  constructor(private profileService: ProfileService, private tokenService: TokenService, private router: Router) { }

  ngOnInit(): void {
    //const idPersona = this.tokenService.getNameIdentifier();
    //if (!idPersona) {
      //console.error('No se pudo obtener el ID de la persona desde el token.', idPersona);
      //return;
    //}
    this.cargarEmprendimientos();
  }

  cargarEmprendimientos(): void {
    this.profileService.obtenerEmprendimientosPorPersona(this.idPersona).subscribe({
      next: (data) => {
        this.emprendimientos = data.map((emprendimiento) => {
          if (emprendimiento.imagen) {
            emprendimiento.imagen = `http://localhost:5145/media/${emprendimiento.imagen}`;
          }
          return emprendimiento; 
        });
      },
      error: (err) => console.error('Error al cargar emprendimientos:', err)
    });
  }

  toEditar(idEmprendimiento: number) {
    this.router.navigate(['/editar-emprendimiento/', idEmprendimiento]);
  }

  eliminarEmprendimiento(idEmprendimiento: number) {
    const confirmar = confirm('¿Estás segura de que deseas eliminar este emprendimiento? Esta acción no se puede deshacer.')
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
