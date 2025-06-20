import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PendingRequestService } from '../../shared/services/pending-request.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-approve-reject-form',
  templateUrl: './approve-reject-form.component.html',
  styleUrls: ['./approve-reject-form.component.css']
})
export class ApproveRejectFormComponent {
  form: FormGroup;
  actionLabel: string;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ApproveRejectFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private pendingRequestService: PendingRequestService,
    private snackBar: MatSnackBar
  ) {
    this.actionLabel = data.action === 'APPROVE' ? 'Update and Approve' : 'Reject';
    this.form = this.fb.group({
  startDate: [data.request.startDate || '', Validators.required],
  endDate: [data.request.endDate || '', Validators.required],
  guestHouseName: [data.request.guestHouseName || '', Validators.required],
  roomName: [data.request.roomName || '', Validators.required],
  bedNumber: [data.request.bedNumber || '', Validators.required],
  address: [{value: data.request.address || '', disabled: true}],
  gender: [data.request.gender || '', Validators.required],
  price: [data.request.price ?? '', [Validators.required, Validators.min(0)]],
  comments: [data.request.adminRemarks || '']
});
  }

  submitForm() {
    if (this.form.valid) {
      const formValue = this.form.getRawValue();
      const bookingId = this.data.request.id;
      const adminId = 1; // Use the actual admin id!

      const updatePayload = {
        startDate: formValue.startDate,
        endDate: formValue.endDate,
        guestHouseName: formValue.guestHouseName,
        roomName: formValue.roomName,
        bedNumber: formValue.bedNumber,
        address: this.data.request.address,
        gender: formValue.gender,
        price: formValue.price
      };

      // Status must be "Approved" or "Rejected" (not "Pending")!
      const status = this.data.action === 'APPROVE' ? 'Approved' : 'Rejected';
      const approvePayload = {
        bookingId,
        status,
        adminRemarks: formValue.comments || ""
      };

      this.pendingRequestService.updateBooking(bookingId, updatePayload).subscribe({
        next: () => {
          this.pendingRequestService.approveOrRejectRequest(approvePayload, adminId).subscribe({
            next: () => {
              this.snackBar.open(
                status === 'Approved'
                  ? 'Booking updated and approved!'
                  : 'Booking rejected!',
                'Close',
                {
                  duration: 3000,
                  verticalPosition: 'top',
                  horizontalPosition: 'right',
                  panelClass: ['snackbar-success']
                }
              );
              this.dialogRef.close(true);
            },
            error: () => {
              // Update succeeded but approve/reject failed, still close dialog
              this.snackBar.open(
                'Booking updated, but failed to approve/reject booking.',
                'Close',
                {
                  duration: 4000,
                  verticalPosition: 'top',
                  horizontalPosition: 'right',
                  panelClass: ['snackbar-error']
                }
              );
              this.dialogRef.close(true);
            }
          });
        },
        error: () => {
          // Update failed, keep dialog open
          this.snackBar.open(
            'Failed to update booking. Please try again.',
            'Close',
            {
              duration: 4000,
              verticalPosition: 'top',
              horizontalPosition: 'right',
              panelClass: ['snackbar-error']
            }
          );
        }
      });
    }
  }

  closeDialog() {
    this.dialogRef.close(false);
  }
}