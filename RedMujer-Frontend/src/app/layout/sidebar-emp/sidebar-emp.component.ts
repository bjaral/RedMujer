import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MATERIAL_IMPORTS } from '../../shared/material/material';

@Component({
  selector: 'app-sidebar-emp',
  imports: [RouterModule, MATERIAL_IMPORTS],
  templateUrl: './sidebar-emp.component.html',
  styleUrl: './sidebar-emp.component.scss'
})

export class SidebarEmpComponent {

}
