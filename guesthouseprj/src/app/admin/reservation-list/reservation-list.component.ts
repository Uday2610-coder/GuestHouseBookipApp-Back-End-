import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { FormControl } from '@angular/forms';
import { BookingService } from '../../shared/services/booking.service';
import { UpdateBookingFormComponent } from '../update-booking-form/update-booking-form.component'; 
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component'; // path as needed



interface Reservation {
  id: number;
  guestHouseId: number;
  guestHouseName: string;
  roomId: number;
  roomName: string;
  bedId: number;
  bedNumber: string;
  userFullName: string;
  startDate: string;
  endDate: string;
  status: string; // Added status
}

@Component({
  selector: 'app-admin-reservation-list',
  templateUrl: './reservation-list.component.html',
  styleUrls: ['./reservation-list.component.css']
})
export class AdminReservationListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'userFullName', 'guestHouseName', 'startDate', 'endDate', 'roomName', 'bedNumber', 'status', 'actions'];
  dataSource: MatTableDataSource<Reservation> = new MatTableDataSource<Reservation>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  // Filters
  guestHouseFilter = new FormControl('');
  roomFilter = new FormControl('');
  bedFilter = new FormControl('');
  dateRange = new FormControl('');

  // Filter options
  guestHouses: string[] = [];
  rooms: string[] = [];
  beds: string[] = [];

  constructor(private bookingService: BookingService,  private dialog: MatDialog) {}

  ngOnInit() {
    this.loadReservations();
    this.setupFilters();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  // Load reservations dynamically from the backend (only approved/rejected)
  loadReservations() {
    this.bookingService.getHistoryBookings().subscribe({
      next: (data: Reservation[]) => {
        this.dataSource.data = data;
        // Populate filter options dynamically
        this.guestHouses = [...new Set(data.map(item => item.guestHouseName))];
        this.rooms = [...new Set(data.map(item => item.roomName))];
        this.beds = [...new Set(data.map(item => item.bedNumber))];
      },
      error: (err) => {
        console.error('Error loading reservations:', err);
      }
    });
  }

  // Set up table filters
  setupFilters() {
    this.dataSource.filterPredicate = (data: Reservation, filter: string) => {
      const searchStr = JSON.parse(filter);

      const matchesGuestHouse = !searchStr.guestHouse || data.guestHouseName === searchStr.guestHouse;
      const matchesRoom = !searchStr.room || data.roomName === searchStr.room;
      const matchesBed = !searchStr.bed || data.bedNumber === searchStr.bed;

      // Date range filter
      const matchesDateRange =
        !searchStr.dateRange ||
        (!searchStr.dateRange.start && !searchStr.dateRange.end) ||
        (data.startDate >= searchStr.dateRange.start && data.endDate <= searchStr.dateRange.end);

      return matchesGuestHouse && matchesRoom && matchesBed && matchesDateRange;
    };

    this.guestHouseFilter.valueChanges.subscribe(() => this.applyFilters());
    this.roomFilter.valueChanges.subscribe(() => this.applyFilters());
    this.bedFilter.valueChanges.subscribe(() => this.applyFilters());
    this.dateRange.valueChanges.subscribe(() => this.applyFilters());
  }

  // Apply filters to the dataSource
  applyFilters() {
    const filterValue = {
      guestHouse: this.guestHouseFilter.value,
      room: this.roomFilter.value,
      bed: this.bedFilter.value,
      dateRange: this.dateRange.value
    };
    this.dataSource.filter = JSON.stringify(filterValue);
  }

  // Reset all filters
  resetFilters() {
    this.guestHouseFilter.reset();
    this.roomFilter.reset();
    this.bedFilter.reset();
    this.dateRange.reset();
    this.dataSource.filter = '';
  }

  editReservation(reservation: Reservation) {
  const dialogRef = this.dialog.open(UpdateBookingFormComponent, {
    width: '600px',
    data: { booking: reservation }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.loadReservations(); // Refresh the table after update
    }
  });
}

  deleteReservation(reservation: Reservation) {
  const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    width: '350px',
    data: { message: `Are you sure you want to delete reservation with ID: ${reservation.id}?` }
  });

  dialogRef.afterClosed().subscribe((confirmed: boolean) => {
    if (confirmed) {
      this.bookingService.deleteBooking(reservation.id).subscribe({
        next: () => {
          this.loadReservations();
        },
        error: (err) => {
        }
      });
    }
  });
}
}