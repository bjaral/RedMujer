import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleEmprendimientoComponent } from './detalle-emprendimiento.component';

describe('DetalleEmprendimientoComponent', () => {
  let component: DetalleEmprendimientoComponent;
  let fixture: ComponentFixture<DetalleEmprendimientoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetalleEmprendimientoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetalleEmprendimientoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
