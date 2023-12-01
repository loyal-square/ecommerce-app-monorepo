import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SvgAccountDetailsComponent } from './svg-account-details.component';

describe('SvgAccountDetailsComponent', () => {
  let component: SvgAccountDetailsComponent;
  let fixture: ComponentFixture<SvgAccountDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SvgAccountDetailsComponent]
    });
    fixture = TestBed.createComponent(SvgAccountDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
