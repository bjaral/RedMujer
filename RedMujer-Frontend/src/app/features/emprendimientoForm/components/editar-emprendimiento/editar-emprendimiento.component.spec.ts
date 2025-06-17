import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditarEmprendimientoComponent } from './editar-emprendimiento.component';

describe('EditarEmprendimientoComponent', () => {
  let component: EditarEmprendimientoComponent;
  let fixture: ComponentFixture<EditarEmprendimientoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditarEmprendimientoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditarEmprendimientoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
