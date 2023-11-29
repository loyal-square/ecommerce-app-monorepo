import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SvgStoreAccountComponent } from './svg-store-account.component';

describe('SvgStoreAccountComponent', () => {
  let component: SvgStoreAccountComponent;
  let fixture: ComponentFixture<SvgStoreAccountComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SvgStoreAccountComponent]
    });
    fixture = TestBed.createComponent(SvgStoreAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
