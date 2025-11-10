import { Component } from '@angular/core';
import { MATERIAL_IMPORTS } from '../../shared/material/material';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-quienes-somos',
  imports: [...MATERIAL_IMPORTS, RouterModule],
  templateUrl: './quienes-somos.component.html',
  styleUrl: './quienes-somos.component.scss'
})
export class QuienesSomosComponent {

}
