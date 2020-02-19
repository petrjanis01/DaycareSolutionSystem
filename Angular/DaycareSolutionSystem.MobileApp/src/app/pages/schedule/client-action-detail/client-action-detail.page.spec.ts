import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientActionDetailPage } from './client-action-detail.page';

describe('ClientActionDetailPage', () => {
  let component: ClientActionDetailPage;
  let fixture: ComponentFixture<ClientActionDetailPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientActionDetailPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientActionDetailPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
