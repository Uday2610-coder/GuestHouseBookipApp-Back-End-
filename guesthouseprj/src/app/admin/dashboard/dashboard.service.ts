import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private baseUrl = 'http://localhost:5003/api'; 

  constructor(private http: HttpClient) {}

  // Fetch available beds for a room within a date range
  getAvailableBeds(roomId: number, startDate: string, endDate: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/Bed/room/${roomId}/available`, {
      params: { startDate, endDate },
    });
  }

  // Fetch all bookings
  getAllBookings(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Booking`);
  }

  // Fetch pending booking requests
  getPendingRequests(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Booking`, { params: { status: 'Pending' } });
  }
}