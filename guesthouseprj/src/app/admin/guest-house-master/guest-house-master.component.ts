import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatStepper } from '@angular/material/stepper';
import { GuestHouseService } from '../../shared/services/guest-house.service';
import { GuestHouse, Room, Bed } from './guest-house-master.interface';

@Component({
  selector: 'app-guest-house-master',
  templateUrl: './guest-house-master.component.html',
  styleUrls: ['./guest-house-master.component.css']
})
export class GuestHouseMasterComponent implements OnInit {
  guestHouseForm!: FormGroup;
  roomForm!: FormGroup;
  bedForm!: FormGroup;

  guestHouses: GuestHouse[] = [];
  rooms: Room[] = [];
  beds: Bed[] = [];
  dropdownRooms: Room[] = [];

  guestHouseDataSource!: MatTableDataSource<GuestHouse>;
  roomDataSource!: MatTableDataSource<Room>;
  bedDataSource!: MatTableDataSource<Bed>;

  displayedGuestHouseColumns: string[] = ['id', 'name', 'address', 'actions'];
  displayedRoomColumns: string[] = ['id', 'name', 'guestHouseId', 'actions'];
  displayedBedColumns: string[] = ['id', 'bedNumber', 'roomId', 'actions'];

  selectedGuestHouseId: number | null = null;

  @ViewChild('guestHousePaginator') guestHousePaginator!: MatPaginator;
  @ViewChild('roomPaginator') roomPaginator!: MatPaginator;
  @ViewChild('bedPaginator') bedPaginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('stepper') stepper!: MatStepper;

  constructor(
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private guestHouseService: GuestHouseService
  ) {
    this.guestHouseForm = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required]
    });
    this.roomForm = this.fb.group({
      name: ['', Validators.required],
      guestHouseId: ['', Validators.required]
    });
    this.bedForm = this.fb.group({
      bedNumber: ['', Validators.required],
      roomId: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.guestHouseDataSource = new MatTableDataSource(this.guestHouses);
    this.roomDataSource = new MatTableDataSource(this.rooms);
    this.bedDataSource = new MatTableDataSource(this.beds);

    this.loadAllMasterData();
  }

  // --- MASTER LOADER: Loads all guest houses, then all rooms, then all beds ---
  loadAllMasterData() {
    this.guestHouseService.getGuestHouses().subscribe({
      next: (guestHouses: GuestHouse[]) => {
        this.guestHouses = guestHouses;
        this.guestHouseDataSource.data = guestHouses;

        // Clear before loading
        let allRooms: Room[] = [];
        let allBeds: Bed[] = [];
        let roomRequests = 0;
        let finishedRoomRequests = 0;

        if (guestHouses.length === 0) {
          this.rooms = [];
          this.beds = [];
          this.roomDataSource.data = [];
          this.bedDataSource.data = [];
          return;
        }

        guestHouses.forEach(gh => {
          roomRequests++;
          this.guestHouseService.getRoomsByGuestHouse(gh.id).subscribe({
            next: (rooms: Room[]) => {
              allRooms = allRooms.concat(rooms);
              finishedRoomRequests++;

              // If we're done loading all rooms for all guest houses, update table and load all beds
              if (finishedRoomRequests === roomRequests) {
                this.rooms = allRooms;
                this.roomDataSource.data = this.rooms;

                // Now load all beds for all rooms!
                if (this.rooms.length === 0) {
                  this.beds = [];
                  this.bedDataSource.data = [];
                  return;
                }
                let bedRequests = 0;
                let finishedBedRequests = 0;
                this.rooms.forEach(room => {
                  bedRequests++;
                  this.guestHouseService.getBedsByRoom(room.id).subscribe({
                    next: (beds: Bed[]) => {
                      allBeds = allBeds.concat(beds);
                      finishedBedRequests++;
                      if (finishedBedRequests === bedRequests) {
                        this.beds = allBeds;
                        this.bedDataSource.data = this.beds;
                      }
                    },
                    error: () => {
                      finishedBedRequests++;
                      if (finishedBedRequests === bedRequests) {
                        this.beds = allBeds;
                        this.bedDataSource.data = this.beds;
                      }
                    }
                  });
                });
              }
            },
            error: () => {
              finishedRoomRequests++;
              if (finishedRoomRequests === roomRequests) {
                this.rooms = allRooms;
                this.roomDataSource.data = this.rooms;
                // Load beds even if some room loads failed
                if (this.rooms.length === 0) {
                  this.beds = [];
                  this.bedDataSource.data = [];
                  return;
                }
                let bedRequests = 0;
                let finishedBedRequests = 0;
                this.rooms.forEach(room => {
                  bedRequests++;
                  this.guestHouseService.getBedsByRoom(room.id).subscribe({
                    next: (beds: Bed[]) => {
                      allBeds = allBeds.concat(beds);
                      finishedBedRequests++;
                      if (finishedBedRequests === bedRequests) {
                        this.beds = allBeds;
                        this.bedDataSource.data = this.beds;
                      }
                    },
                    error: () => {
                      finishedBedRequests++;
                      if (finishedBedRequests === bedRequests) {
                        this.beds = allBeds;
                        this.bedDataSource.data = this.beds;
                      }
                    }
                  });
                });
              }
            }
          });
        });
      },
      error: () => this.showErrorMessage('Failed to load guest houses')
    });
  }

  // --- Guest House Actions ---
  addGuestHouse() {
    if (this.guestHouseForm.valid) {
      this.guestHouseService.addGuestHouse(this.guestHouseForm.value).subscribe({
        next: (gh: GuestHouse) => {
          this.loadAllMasterData();
          this.guestHouseForm.reset();
          this.showSuccessMessage('Guest house added!');
        },
        error: () => this.showErrorMessage('Failed to add guest house')
      });
    }
  }

  deleteGuestHouse(id: number) {
    this.guestHouseService.deleteGuestHouse(id).subscribe({
      next: () => {
        this.loadAllMasterData();
        this.showSuccessMessage('Guest house and its rooms/beds deleted.');
      },
      error: () => this.showErrorMessage('Failed to delete guest house')
    });
  }

  // --- Room Actions ---
  addRoom() {
    if (this.roomForm.valid) {
      this.guestHouseService.addRoom(this.roomForm.value).subscribe({
        next: (room: Room) => {
          this.loadAllMasterData();
          this.roomForm.reset();
          this.showSuccessMessage('Room added!');
        },
        error: () => this.showErrorMessage('Failed to add room')
      });
    }
  }

  deleteRoom(id: number) {
    this.guestHouseService.deleteRoom(id).subscribe({
      next: () => {
        this.loadAllMasterData();
        this.showSuccessMessage('Room and its beds deleted.');
      },
      error: () => this.showErrorMessage('Failed to delete room')
    });
  }

  // --- Bed Actions ---
  addBed() {
    if (this.bedForm.valid) {
      this.guestHouseService.addBed(this.bedForm.value).subscribe({
        next: (bed: Bed) => {
          this.loadAllMasterData();
          this.bedForm.reset();
          this.showSuccessMessage('Bed added!');
        },
        error: () => this.showErrorMessage('Failed to add bed')
      });
    }
  }

  deleteBed(id: number) {
    this.guestHouseService.deleteBed(id).subscribe({
      next: () => {
        this.loadAllMasterData();
        this.showSuccessMessage('Bed deleted.');
      },
      error: () => this.showErrorMessage('Failed to delete bed')
    });
  }

  // --- Dropdown logic for Add Bed form ---
  onGuestHouseSelected(event: any) {
    const guestHouseId = event.value;
    this.selectedGuestHouseId = guestHouseId;
    this.guestHouseService.getRoomsByGuestHouse(guestHouseId).subscribe({
      next: (rooms: Room[]) => {
        this.dropdownRooms = rooms;
        this.bedForm.get('roomId')?.setValue('');
      },
      error: () => this.showErrorMessage('Failed to load rooms')
    });
  }

  getGuestHouseNameById(id: number): string {
    const gh = this.guestHouses.find(g => g.id === id);
    return gh ? gh.name : id.toString();
  }

  getRoomNameById(id: number): string {
    const room = this.rooms.find(r => r.id === id);
    return room ? room.name : id.toString();
  }

  private showSuccessMessage(msg: string) {
    this.snackBar.open(msg, 'Close', { duration: 2000 });
  }
  private showErrorMessage(msg: string) {
    this.snackBar.open(msg, 'Close', { duration: 2500, panelClass: ['mat-warn'] });
  }





  modalOpen = false;
  modalMessage = '';
  modalDeleteAction: (() => void) | null = null;

  openConfirm(message: string, onConfirm: () => void) {
    this.modalMessage = message;
    this.modalDeleteAction = onConfirm;
    this.modalOpen = true;
  }
  modalConfirm() {
    if (this.modalDeleteAction) {
      this.modalDeleteAction();
    }
    this.modalOpen = false;
  }

  // For Guest House
  confirmDeleteGuestHouse(id: number) {
    this.openConfirm(
      `Are you sure you want to delete this Guest House and all its Rooms/Beds?`,
      () => this.deleteGuestHouse(id)
    );
  }

  // For Room
  confirmDeleteRoom(id: number) {
    this.openConfirm(
      `Are you sure you want to delete this Room and its Beds?`,
      () => this.deleteRoom(id)
    );
  }

  // For Bed
  confirmDeleteBed(id: number) {
    this.openConfirm(
      `Are you sure you want to delete this Bed?`,
      () => this.deleteBed(id)
    );
  }


}