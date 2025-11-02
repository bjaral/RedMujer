import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MATERIAL_IMPORTS } from '../../shared/material/material';

@Component({
  selector: 'app-creditos',
  imports: [CommonModule, ...MATERIAL_IMPORTS],
  templateUrl: './creditos.component.html',
  styleUrl: './creditos.component.scss'
})
export class CreditosComponent {

}
