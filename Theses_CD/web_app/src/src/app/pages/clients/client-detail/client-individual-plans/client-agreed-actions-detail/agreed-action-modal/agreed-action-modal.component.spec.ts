import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgreedActionModalComponent } from './agreed-action-modal.component';

describe('AgreedActionModalComponent', () => {
  let component: AgreedActionModalComponent;
  let fixture: ComponentFixture<AgreedActionModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgreedActionModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgreedActionModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
