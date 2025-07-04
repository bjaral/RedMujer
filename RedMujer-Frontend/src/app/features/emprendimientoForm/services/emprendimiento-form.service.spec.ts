import { TestBed } from '@angular/core/testing';

import { EmprendimientoFormService } from './emprendimiento-form.service';

describe('EmprendimientoFormService', () => {
  let service: EmprendimientoFormService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EmprendimientoFormService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
