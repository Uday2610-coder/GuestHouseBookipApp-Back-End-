import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface BookingReport {
  id: number;
  userFullName: string;
  address: string;
  guestHouseName: string;
  roomName: string;
  bedNumber: string;
  startDate: string;
  endDate: string;
  status: string;
}

@Injectable({ providedIn: 'root' })
export class BookingReportService {
  private baseUrl = 'http://localhost:5003/api/Booking';

  constructor(private http: HttpClient) {}

  // Use the /report endpoint for filtered bookings (for the report page)
  getBookings(filters: any): Observable<BookingReport[]> {
    let params = new HttpParams();
    Object.entries(filters).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, String(value));
      }
    });
    return this.http.get<BookingReport[]>(`${this.baseUrl}/report`, { params });
  }

  exportBookings(filters: any): Observable<Blob> {
    let params = new HttpParams();
    Object.entries(filters).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, String(value));
      }
    });
    return this.http.get(`${this.baseUrl}/export`, { params, responseType: 'blob' });
  }
}