import { Component, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MATERIAL_IMPORTS } from '../../shared/material/material';

@Component({
  selector: 'app-not-found',
  imports: [CommonModule, RouterModule, ...MATERIAL_IMPORTS],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss'
})
export class NotFoundComponent {
  @ViewChild('tituloRef') tituloRef!: ElementRef<HTMLHeadingElement>;
  @ViewChild('contenedorRef') contenedorRef!: ElementRef<HTMLElement>;
  @ViewChild('parrafoRef') parrafoRef!: ElementRef<HTMLElement>;

  ngAfterViewInit(): void {
    if (this.tituloRef && this.contenedorRef && this.parrafoRef) {
      const h2Width = this.tituloRef.nativeElement.offsetWidth;

      this.contenedorRef.nativeElement.style.width = `${h2Width}px`;
      this.parrafoRef.nativeElement.style.width = `${h2Width}px`;

      const ilustracionDiv = this.contenedorRef.nativeElement.querySelector('.login-ilustracion');
      if (ilustracionDiv) {
        (ilustracionDiv as HTMLElement).style.width = `${h2Width}px`;
      }
    }
  }
}
