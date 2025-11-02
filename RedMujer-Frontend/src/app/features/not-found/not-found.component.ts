import { Component } from '@angular/core';
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

}
