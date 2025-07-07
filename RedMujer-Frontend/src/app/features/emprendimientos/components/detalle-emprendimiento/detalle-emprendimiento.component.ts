import { Component } from '@angular/core';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { EmprendimientoService } from '../../services/emprendimiento.service';

@Component({
  selector: 'app-detalle-emprendimiento',
  imports: [MATERIAL_IMPORTS, CommonModule],
  templateUrl: './detalle-emprendimiento.component.html',
  styleUrl: './detalle-emprendimiento.component.scss'
})


export class DetalleEmprendimientoComponent {
  idEmprendimiento!: number;

  nombre = "";
  categoria = "";
  Instagram = "";
  Facebook = "";
  X = "";
  Whatsapp = "";

  web = "";
  ubicacion = "";

  constructor(private emprendimeinto: EmprendimientoService ,private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.idEmprendimiento = +params['id'];
    });
    this.getEmprendimiento();
  }

  getEmprendimiento() {
    this.emprendimeinto.getById(this.idEmprendimiento).subscribe({
      next: (data) => {
        this.nombre = data.nombre;
      }
    })
  }
}
