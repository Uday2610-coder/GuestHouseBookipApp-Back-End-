import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GuestHouse, Room, Bed } from '../../admin/guest-house-master/guest-house-master.interface';

@Injectable({ providedIn: 'root' })
export class GuestHouseService {
  private apiUrl = 'http://localhost:5003/api';

  constructor(private http: HttpClient) {}

  getGuestHouses(): Observable<GuestHouse[]> {
    return this.http.get<GuestHouse[]>(`${this.apiUrl}/GuestHouse`);
  }

  addGuestHouse(model: Partial<GuestHouse>): Observable<GuestHouse> {
    return this.http.post<GuestHouse>(`${this.apiUrl}/GuestHouse`, model);
  }

  deleteGuestHouse(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/GuestHouse/${id}`);
  }

  getRoomsByGuestHouse(guestHouseId: number): Observable<Room[]> {
    return this.http.get<Room[]>(`${this.apiUrl}/Room/guesthouse/${guestHouseId}`);
  }

  addRoom(model: Partial<Room>): Observable<Room> {
    return this.http.post<Room>(`${this.apiUrl}/Room`, model);
  }

  deleteRoom(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Room/${id}`);
  }

  getBedsByRoom(roomId: number): Observable<Bed[]> {
    return this.http.get<Bed[]>(`${this.apiUrl}/Bed/room/${roomId}`);
  }

  addBed(model: Partial<Bed>): Observable<Bed> {
    return this.http.post<Bed>(`${this.apiUrl}/Bed`, model);
  }

  deleteBed(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Bed/${id}`);
  }

  getAllRooms(): Observable<Room[]> {
    return this.http.get<Room[]>(`${this.apiUrl}/Room`);
  }

  getAllBeds(): Observable<Bed[]> {
    return this.http.get<Bed[]>(`${this.apiUrl}/Bed`);
  }
}