import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OnInit } from '@angular/core';
import { HomeService } from '../../services/home.service';
import { MATERIAL_IMPORTS } from '../../../../shared/material/material';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [CommonModule, ...MATERIAL_IMPORTS],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  random: any;

  constructor(private homeService: HomeService, private router: Router) {}

  ngOnInit(): void {
    this.getRandom();
  }

  getRandom(): void {
    this.homeService.getRandom().subscribe(
      data => {
        this.random = data;
      },
      error => {
        console.error('Error al obtener datos:', error);
      }
    );
  }

  toEmprendimientos() {
    this.router.navigate(['/emprendimientos']);
  }
  
}