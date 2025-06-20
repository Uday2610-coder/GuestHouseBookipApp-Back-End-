import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private apiUrl = 'http://localhost:5003/api/Booking'; // Backend API URL

  constructor(private http: HttpClient) {}

  // Fetch only approved or rejected bookings for history
  getHistoryBookings(): Observable<any[]> {
    const url = `${this.apiUrl}?status=Approved,Rejected`;
    return this.http.get<any[]>(url).pipe(
      catchError(this.handleError)
    );
  }

  // Update booking by ID
  updateBooking(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, data).pipe(
      catchError(this.handleError)
    );
  }

  // Delete booking by ID
  deleteBooking(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Error handling
  private handleError(error: HttpErrorResponse) {
    console.error('Backend returned an error:', error.message);
    return throwError(() => new Error('Unable to process request. Please try again later.'));
  }
}