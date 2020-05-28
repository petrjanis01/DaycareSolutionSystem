import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientGeneralInfoComponent } from './client-general-info.component';

describe('ClientGeneralInfoComponent', () => {
  let component: ClientGeneralInfoComponent;
  let fixture: ComponentFixture<ClientGeneralInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientGeneralInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientGeneralInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
