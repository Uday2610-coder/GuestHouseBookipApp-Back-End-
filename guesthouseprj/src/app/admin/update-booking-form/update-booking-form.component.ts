import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { BookingService } from '../../shared/services/booking.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-update-booking-form',
  templateUrl: './update-booking-form.component.html',
  styleUrls: ['./update-booking-form.component.css']
})
export class UpdateBookingFormComponent {
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<UpdateBookingFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private bookingService: BookingService,
    private snackBar: MatSnackBar
  ) {
    this.form = this.fb.group({
      startDate: [data.booking.startDate || '', Validators.required],
      endDate: [data.booking.endDate || '', Validators.required],
      guestHouseName: [data.booking.guestHouseName || '', Validators.required],
      roomName: [data.booking.roomName || '', Validators.required],
      bedNumber: [data.booking.bedNumber || '', Validators.required],
      address: [{value: data.booking.address || '', disabled: true}],
      gender: [data.booking.gender || '', Validators.required],
      price: [data.booking.price ?? '', [Validators.required, Validators.min(0)]]
    });
  }

  submitForm() {
    if (this.form.valid) {
      const formValue = this.form.getRawValue();
      this.bookingService.updateBooking(this.data.booking.id, formValue).subscribe({
        next: () => {
          this.snackBar.open('Booking updated!', 'Close', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'right',
            panelClass: ['snackbar-success']
          });
          this.dialogRef.close(true);
        },
        error: () => {
          this.snackBar.open('Failed to update booking.', 'Close', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'right',
            panelClass: ['snackbar-error']
          });
        }
      });
    }
  }

  closeDialog() {
    this.dialogRef.close(false);
  }
}