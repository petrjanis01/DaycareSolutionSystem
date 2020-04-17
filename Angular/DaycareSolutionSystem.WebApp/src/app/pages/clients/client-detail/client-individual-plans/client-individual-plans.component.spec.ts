import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientIndividualPlansComponent } from './client-individual-plans.component';

describe('ClientIndividualPlansComponent', () => {
  let component: ClientIndividualPlansComponent;
  let fixture: ComponentFixture<ClientIndividualPlansComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientIndividualPlansComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientIndividualPlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
