import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateBookingFormComponent } from './update-booking-form.component';

describe('UpdateBookingFormComponent', () => {
  let component: UpdateBookingFormComponent;
  let fixture: ComponentFixture<UpdateBookingFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UpdateBookingFormComponent]
    });
    fixture = TestBed.createComponent(UpdateBookingFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
