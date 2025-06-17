import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminEditarEmprendimientosComponent } from './admin-editar-emprendimientos.component';

describe('AdminEditarEmprendimientosComponent', () => {
  let component: AdminEditarEmprendimientosComponent;
  let fixture: ComponentFixture<AdminEditarEmprendimientosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminEditarEmprendimientosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminEditarEmprendimientosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
