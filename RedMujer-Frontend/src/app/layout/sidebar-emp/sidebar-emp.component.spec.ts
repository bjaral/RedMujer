import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidebarEmpComponent } from './sidebar-emp.component';

describe('SidebarEmpComponent', () => {
  let component: SidebarEmpComponent;
  let fixture: ComponentFixture<SidebarEmpComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SidebarEmpComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SidebarEmpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
