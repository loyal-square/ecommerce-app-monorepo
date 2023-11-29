import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SvgVerifyComponent } from './svg-verify.component';

describe('SvgVerifyComponent', () => {
  let component: SvgVerifyComponent;
  let fixture: ComponentFixture<SvgVerifyComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SvgVerifyComponent]
    });
    fixture = TestBed.createComponent(SvgVerifyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
