import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { BookingDataService } from '../../shared/services/booking-data.service';
import { AuthService } from '../../shared/services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

interface Booking {
  id: number;
  guestHouseName: string;
  address: string;
  roomName: string;
  bedNumber: string;
  startDate: string;
  endDate: string;
  purpose: string;
  status: string;
}

@Component({
  selector: 'app-mybookings',
  templateUrl: './mybookings.component.html',
  styleUrls: ['./mybookings.component.css']
})
export class MybookingsComponent implements OnInit {
  displayedColumns: string[] = [
    'id', 'guestHouseName', 'address', 'roomName', 'bedNumber',
    'startDate', 'endDate', 'purpose', 'status', 'actions'
  ];
  dataSource = new MatTableDataSource<Booking>([]);
  rawBookings: Booking[] = [];

  guestHouses: string[] = [];
  locations: string[] = [];
  statuses: string[] = [];

  filter = {
    guestHouse: '',
    location: '',
    status: '',
    global: ''
  };

  @ViewChild('deleteDialog') deleteDialog!: TemplateRef<any>;
  selectedBookingId: number | null = null;

  constructor(
    private dialog: MatDialog,
    private bookingDataService: BookingDataService,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) {
    this.dataSource.filterPredicate = this.createFilter();
  }

  ngOnInit() {
    const userId = this.authService.getCurrentUserId();
    if (!userId) return;

    this.bookingDataService.getBookingsByUser(userId).subscribe(bookings => {
      this.rawBookings = bookings;
      this.dataSource.data = this.rawBookings;

      // Dynamically get unique filter values
      this.guestHouses = Array.from(new Set(bookings.map(b => b.guestHouseName || b.guestHouse))).filter(Boolean);
      this.locations = Array.from(new Set(bookings.map(b => b.address || b.location))).filter(Boolean);
      this.statuses = Array.from(new Set(bookings.map(b => b.status))).filter(Boolean);
    });
  }

  applyGlobalFilter(event: Event) {
    this.filter.global = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.dataSource.filter = JSON.stringify(this.filter);
  }

  applyDropdownFilter() {
    this.dataSource.filter = JSON.stringify(this.filter);
  }

  createFilter() {
    return (data: any, filter: string): boolean => {
      const search = JSON.parse(filter);

      const globalMatch = Object.values(data)
        .some(val => val != null && val.toString().toLowerCase().includes(search.global));

      const guestMatch = !search.guestHouse || data.guestHouseName === search.guestHouse;
      const locationMatch = !search.location || data.address === search.location;
      const statusMatch = !search.status || data.status === search.status;

      return globalMatch && guestMatch && locationMatch && statusMatch;
    };
  }

  openDeleteDialog(id: number) {
    this.selectedBookingId = id;
    const dialogRef = this.dialog.open(this.deleteDialog);

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'delete') {
        this.bookingDataService.deleteBooking(id).subscribe({
          next: () => {
            this.rawBookings = this.rawBookings.filter(b => b.id !== id);
            this.dataSource.data = this.rawBookings;
            // Update statuses if you use dynamic filters
            this.statuses = Array.from(new Set(this.rawBookings.map(b => b.status))).filter(Boolean);
            this.snackBar.open('Booking deleted.', 'Close', { 
              verticalPosition: 'top', horizontalPosition: 'right', panelClass: ['success-snackbar'],
              duration: 2000 });
          },
          error: () => {
            this.snackBar.open('Failed to delete booking.', 'Close', { duration: 2000 });
          }
        });
      }
      this.selectedBookingId = null;
    });
  }
}