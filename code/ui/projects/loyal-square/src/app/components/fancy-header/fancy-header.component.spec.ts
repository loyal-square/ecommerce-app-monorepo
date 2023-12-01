import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FancyHeaderComponent } from './fancy-header.component';

describe('FancyHeaderComponent', () => {
  let component: FancyHeaderComponent;
  let fixture: ComponentFixture<FancyHeaderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FancyHeaderComponent]
    });
    fixture = TestBed.createComponent(FancyHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
