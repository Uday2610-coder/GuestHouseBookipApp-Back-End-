import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/shared/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BookingDataService } from '../../shared/services/booking-data.service';

@Component({
  selector: 'app-booking-page',
  templateUrl: './booking-page.component.html',
  styleUrls: ['./booking-page.component.css']
})
export class BookingPageComponent implements OnInit {
  bookingForm: FormGroup;

  guestHouses: any[] = [];
  rooms: any[] = [];
  beds: any[] = [];
  genders = ['Male', 'Female', 'Other'];

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private bookingDataService: BookingDataService,
    private snackBar: MatSnackBar // <-- Inject MatSnackBar
  ) {
    this.bookingForm = this.fb.group({
      checkIn: ['', Validators.required],
      checkOut: ['', Validators.required],
      guestHouse: ['', Validators.required],
      location: [{ value: '', disabled: true }, Validators.required],
      roomNo: ['', Validators.required],
      bedNo: ['', Validators.required],
      gender: ['', Validators.required],
      purpose: ['', Validators.required],
      acceptTerms: [false, Validators.requiredTrue]
    });
  }

  ngOnInit() {
    this.bookingDataService.getGuestHouses().subscribe(data => {
      this.guestHouses = data;
    });
  }

  onGuestHouseChange(event: any) {
    const guestHouseId = event.value;
    const selectedHouse = this.guestHouses.find(gh => gh.id === guestHouseId);
    this.bookingForm.patchValue({ location: selectedHouse ? selectedHouse.address : '' });
    this.bookingDataService.getRoomsByGuestHouse(guestHouseId).subscribe(data => {
      this.rooms = data;
      this.bookingForm.patchValue({ roomNo: '', bedNo: '' });
      this.beds = [];
    });
  }

  onRoomChange(event: any) {
    const roomId = event.value;
    this.bookingDataService.getBedsByRoom(roomId).subscribe(data => {
      this.beds = data;
      this.bookingForm.patchValue({ bedNo: '' });
    });
  }

  onSubmit() {
  if (this.bookingForm.valid) {
    const userId = this.authService.getCurrentUserId();
    if (userId === null) {
      this.snackBar.open('User ID not found. Please log in again.', 'Close', {
        duration: 3000
      });
      return;
    }

    const formRaw = this.bookingForm.getRawValue();
    const booking = {
      guestHouseId: formRaw.guestHouse,
      roomId: formRaw.roomNo,
      bedId: formRaw.bedNo,
      startDate: formRaw.checkIn,
      endDate: formRaw.checkOut,
      address: formRaw.location, 
      price: 0,
      gender: formRaw.gender,
      purpose: formRaw.purpose,
    };

    console.log('Booking payload:', booking);

    this.bookingDataService.createBooking(userId, booking).subscribe({
      next: () => {
         this.snackBar.open('Booking created successfully!', 'Close', { 
          
          verticalPosition: 'top',
          horizontalPosition: 'right',
          duration: 3000 });
        this.bookingForm.reset(); 
        this.rooms = [];
        this.beds = [];
       },
      error: () => {
         this.snackBar.open('Failed to create booking.', 'Close', { 
          verticalPosition: 'top',
          horizontalPosition: 'right',
          duration: 3000 });
       }
    });
  } else {
    this.bookingForm.markAllAsTouched();
  }
}

  onCancel() {
    this.bookingForm.reset();
  }
}