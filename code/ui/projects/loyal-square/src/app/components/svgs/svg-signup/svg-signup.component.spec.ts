import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SvgSignupComponent } from './svg-signup.component';

describe('SvgSignupComponent', () => {
  let component: SvgSignupComponent;
  let fixture: ComponentFixture<SvgSignupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SvgSignupComponent]
    });
    fixture = TestBed.createComponent(SvgSignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
