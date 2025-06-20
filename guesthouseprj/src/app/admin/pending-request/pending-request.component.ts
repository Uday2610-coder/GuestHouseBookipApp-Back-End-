import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { FormControl } from '@angular/forms';
import { PendingRequestService, PendingRequest } from '../../shared/services/pending-request.service';
import { ApproveRejectFormComponent } from '../approve-reject-form/approve-reject-form.component';

@Component({
  selector: 'app-pending-request',
  templateUrl: './pending-request.component.html',
  styleUrls: ['./pending-request.component.css']
})
export class PendingRequestComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = [
    'id',
    'userFullName',
    'guestHouseName',
    'startDate',
    'endDate',
    'roomName',
    'bedNumber',
    'status',
    'actions'
  ];
  dataSource: MatTableDataSource<PendingRequest> = new MatTableDataSource<PendingRequest>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  guestHouseFilter = new FormControl('');
  roomFilter = new FormControl('');
  bedFilter = new FormControl('');
  employeeFilter = new FormControl('');
  checkInStart = new FormControl();
  checkInEnd = new FormControl();

  guestHouses: string[] = [];
  rooms: string[] = [];
  beds: string[] = [];

  allPending: PendingRequest[] = [];

  constructor(
    private pendingRequestService: PendingRequestService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.loadPendingRequests();
    this.setupFilters();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  loadPendingRequests(): void {
    this.pendingRequestService.getPendingRequests().subscribe({
      next: (data: PendingRequest[]) => {
        const pendingOnly = data.filter(
          r => r.status && r.status.trim().toLowerCase() === 'pending'
        );
        this.allPending = pendingOnly;
        this.dataSource.data = pendingOnly;
        this.updateFilterOptions(pendingOnly);
      },
      error: (err) => {
        console.error('Error loading pending requests:', err);
        this.dataSource.data = [];
        this.updateFilterOptions([]);
      }
    });
  }

  updateFilterOptions(data: PendingRequest[]) {
    this.guestHouses = Array.from(new Set(data.map(r => r.guestHouseName).filter(Boolean)));
    this.rooms = Array.from(new Set(data.map(r => r.roomName).filter(Boolean)));
    this.beds = Array.from(new Set(data.map(r => r.bedNumber).filter(Boolean)));
  }

  setupFilters(): void {
    this.dataSource.filterPredicate = (data: PendingRequest, filter: string) => {
      const searchStr = JSON.parse(filter);

      const matchesGuestHouse = !searchStr.guestHouse || data.guestHouseName === searchStr.guestHouse;
      const matchesRoom = !searchStr.room || data.roomName === searchStr.room;
      const matchesBed = !searchStr.bed || data.bedNumber === searchStr.bed;
      const matchesUser = !searchStr.user || data.userFullName?.toLowerCase().includes(searchStr.user.toLowerCase());

      let matchesDate = true;
      if (searchStr.checkInStart) {
        matchesDate = matchesDate && new Date(data.startDate) >= new Date(searchStr.checkInStart);
      }
      if (searchStr.checkInEnd) {
        matchesDate = matchesDate && new Date(data.startDate) <= new Date(searchStr.checkInEnd);
      }

      return matchesGuestHouse && matchesRoom && matchesBed && matchesUser && matchesDate;
    };

    this.guestHouseFilter.valueChanges.subscribe(() => this.applyFilters());
    this.roomFilter.valueChanges.subscribe(() => this.applyFilters());
    this.bedFilter.valueChanges.subscribe(() => this.applyFilters());
    this.employeeFilter.valueChanges.subscribe(() => this.applyFilters());
    this.checkInStart.valueChanges.subscribe(() => this.applyFilters());
    this.checkInEnd.valueChanges.subscribe(() => this.applyFilters());
  }

  applyFilters(): void {
    const filterValue = {
      guestHouse: this.guestHouseFilter.value || '',
      room: this.roomFilter.value || '',
      bed: this.bedFilter.value || '',
      user: this.employeeFilter.value || '',
      checkInStart: this.checkInStart.value ? this.checkInStart.value : '',
      checkInEnd: this.checkInEnd.value ? this.checkInEnd.value : ''
    };
    this.dataSource.filter = JSON.stringify(filterValue);
  }

  resetFilters(): void {
    this.guestHouseFilter.setValue('');
    this.roomFilter.setValue('');
    this.bedFilter.setValue('');
    this.employeeFilter.setValue('');
    this.checkInStart.setValue('');
    this.checkInEnd.setValue('');
    this.applyFilters();
  }

  openApproveForm(request: PendingRequest): void {
    this.pendingRequestService.getBookingById(request.id).subscribe(fullBooking => {
      const dialogRef = this.dialog.open(ApproveRejectFormComponent, {
        width: '600px',
        data: { request: fullBooking, action: 'APPROVE' },
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.loadPendingRequests();
        }
      });
    });
  }

  openRejectForm(request: PendingRequest): void {
    this.pendingRequestService.getBookingById(request.id).subscribe(fullBooking => {
      const dialogRef = this.dialog.open(ApproveRejectFormComponent, {
        width: '600px',
        data: { request: fullBooking, action: 'REJECT' },
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.loadPendingRequests();
        }
      });
    });
  }
  
}