import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { BookingReportService, BookingReport } from 'src/app/shared/services/booking-report.service';

@Component({
  selector: 'app-admin-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class AdminReportComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = [
    'serialNo', 'userFullName', 'address', 'guestHouseName', 'roomName', 'bedNumber', 'startDate', 'endDate', 'status'
  ];
  dataSource = new MatTableDataSource<BookingReport>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  locationFilter = new FormControl('');
  guestHouseFilter = new FormControl('');
  statusFilter = new FormControl('');

  dateRangeGroup = new FormGroup({
  start: new FormControl<Date | null>(null),
  end: new FormControl<Date | null>(null)
  });

  locations: string[] = [];
  guestHouses: string[] = [];
  statuses: string[] = ['Pending', 'Approved', 'Rejected'];

  constructor(private bookingReportService: BookingReportService) {}

  ngOnInit() {
    this.loadFilterOptions();
    this.loadData();
    this.setupFilters();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  setupFilters() {
    this.locationFilter.valueChanges.subscribe(() => this.loadData());
    this.guestHouseFilter.valueChanges.subscribe(() => this.loadData());
    this.statusFilter.valueChanges.subscribe(() => this.loadData());
    this.dateRangeGroup.valueChanges.subscribe(() => this.loadData());
  }

  getFilterValues() {
    let startDate = '';
    let endDate = '';
    if (this.dateRangeGroup.value && this.dateRangeGroup.value.start && this.dateRangeGroup.value.end) {
      startDate = this.dateRangeGroup.value.start.toISOString().split('T')[0];
      endDate = this.dateRangeGroup.value.end.toISOString().split('T')[0];
    }
    return {
      address: this.locationFilter.value || undefined,
      guestHouseName: this.guestHouseFilter.value || undefined,
      status: this.statusFilter.value || undefined,
      startDate: startDate || undefined,
      endDate: endDate || undefined
    };
  }

  loadData() {
    const filters = this.getFilterValues();
    console.log('Filters being sent:', filters);
    this.bookingReportService.getBookings(filters).subscribe(data => {
      this.dataSource.data = data || [];
    });
  }

  resetFilters() {
    this.locationFilter.reset('');
    this.guestHouseFilter.reset('');
    this.statusFilter.reset('');
    this.dateRangeGroup.reset({start: null, end: null});
    this.loadData();
  }

  exportReport() {
    const filters = this.getFilterValues();
    this.bookingReportService.exportBookings(filters).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'bookings_report.csv';
      a.click();
      window.URL.revokeObjectURL(url);
    });
  }

  loadFilterOptions() {
    // For option selection, you may want all data, but for performance, you could use distinct endpoints if available
    this.bookingReportService.getBookings({}).subscribe(data => {
      this.locations = Array.from(new Set(data.map(r => r.address).filter(Boolean)));
      this.guestHouses = Array.from(new Set(data.map(r => r.guestHouseName).filter(Boolean)));
    });
  }
}