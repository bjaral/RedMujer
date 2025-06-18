import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OnInit } from '@angular/core';
import { HomeService } from '../../services/home.service';

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  random: any;

  constructor(private homeService: HomeService) {}

  ngOnInit(): void {
    this.getRandom();
  }

  getRandom(): void {
    this.homeService.getRandom().subscribe(
      data => {
        this.random = data;
        console.log('Respuesta recibida:', data);
      },
      error => {
        console.error('Error al obtener datos:', error);
      }
    );
  }
  
}