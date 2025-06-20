import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class BookingDataService {
  constructor(private http: HttpClient) {}

  // GET /api/GuestHouse
  getGuestHouses(): Observable<any[]> {
    return this.http.get<any[]>('/api/GuestHouse');
  }

  // GET /api/Room/guesthouse/{guesthouseId}
  getRoomsByGuestHouse(guestHouseId: number): Observable<any[]> {
    return this.http.get<any[]>(`/api/Room/guesthouse/${guestHouseId}`);
  }

  // GET /api/Bed/room/{roomId}
  getBedsByRoom(roomId: number): Observable<any[]> {
    return this.http.get<any[]>(`/api/Bed/room/${roomId}`);
  }
  // POST /api/Booking
 
  createBooking(userId: number, booking: any): Observable<any> {
  return this.http.post<any>(
    `/api/Booking?userId=${userId}`,
    booking
  );
  }
   getBookingsByUser(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`/api/Booking/user/${userId}`);
  }

  deleteBooking(bookingId: number): Observable<any> {
    return this.http.delete<any>(`/api/Booking/${bookingId}`);
  }
  
}