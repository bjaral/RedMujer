import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MisEmprendimientosComponent } from './mis-emprendimientos.component';

describe('MisEmprendimientosComponent', () => {
  let component: MisEmprendimientosComponent;
  let fixture: ComponentFixture<MisEmprendimientosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MisEmprendimientosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MisEmprendimientosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
