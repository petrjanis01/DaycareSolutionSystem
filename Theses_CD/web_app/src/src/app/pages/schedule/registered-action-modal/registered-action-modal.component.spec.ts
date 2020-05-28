import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisteredActionModalComponent } from './registered-action-modal.component';

describe('RegisteredActionModalComponent', () => {
  let component: RegisteredActionModalComponent;
  let fixture: ComponentFixture<RegisteredActionModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegisteredActionModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisteredActionModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
