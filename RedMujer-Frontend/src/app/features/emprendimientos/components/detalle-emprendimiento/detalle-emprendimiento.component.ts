import { Component } from '@angular/core';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { EmprendimientoService } from '../../services/emprendimiento.service';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-detalle-emprendimiento',
  imports: [MATERIAL_IMPORTS, CommonModule, MatButtonModule, MatIconModule],
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
  urlImagen = "";
  ubicacion = "";

  links = []

  constructor(private emprendimiento: EmprendimientoService ,private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.idEmprendimiento = +params['id'];
    });
    this.getEmprendimiento();
  }

  getEmprendimiento() {
    this.emprendimiento.getById(this.idEmprendimiento).subscribe({
      next: (data) => {
        this.nombre = data.nombre;
      }
    })
    this.emprendimiento.getByIdImg(this.idEmprendimiento).subscribe({
      next: (data) => {
        this.urlImagen = data.imagenes[0];
      }
    })
    this.emprendimiento.getByIdRedes(this.idEmprendimiento).subscribe({
      next: (data) => {
        this.links = data.valor;
      }
    })
  }




  @ViewChild('myVideo') videoRef!: ElementRef<HTMLVideoElement>;

  playVideo() {
    const video: HTMLVideoElement = this.videoRef.nativeElement;
    if (video.paused) {
      video.play();    // Reproduce el video
    } else {
      video.pause();   // Pausa el video si está reproduciendo
    }
  }

  onVideoPlay() {
    const video: HTMLVideoElement = this.videoRef.nativeElement;
    const playButton = document.querySelector('.play-button');
    playButton?.classList.add('hidden'); // Oculta el botón al comenzar a reproducir
  }
}
