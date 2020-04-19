import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAgreedActionsDetailComponent } from './client-agreed-actions-detail.component';

describe('ClientAgreedActionsDetailComponent', () => {
  let component: ClientAgreedActionsDetailComponent;
  let fixture: ComponentFixture<ClientAgreedActionsDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAgreedActionsDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAgreedActionsDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
