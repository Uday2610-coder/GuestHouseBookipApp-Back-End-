import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface PendingRequest {
  id: number;
  guestHouseId: number;
  guestHouseName: string;
  roomId: number;
  roomName: string;
  bedId: number;
  bedNumber: string;
  userId: number;
  userFullName: string;
  startDate: string;
  endDate: string;
  status: string;
  adminRemarks?: string;
  address?: string;
  gender?: string;
}

@Injectable({
  providedIn: 'root'
})
export class PendingRequestService {
  private apiUrl = 'http://localhost:5003/api/Booking';

  constructor(private http: HttpClient) {}

  /**
   * Get all pending booking requests.
   * Adds a cache-busting query param to ensure fresh data.
   */
  getPendingRequests(): Observable<PendingRequest[]> {
    const params = new HttpParams().set('_ts', Date.now().toString());
    return this.http.get<PendingRequest[]>(`${this.apiUrl}`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Get a single booking by ID.
   */
  getBookingById(id: number): Observable<PendingRequest> {
    return this.http.get<PendingRequest>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Update booking fields for a specific booking.
   * @param id Booking ID
   * @param payload Object with fields to update
   */
  updateBooking(id: number, payload: Partial<PendingRequest>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, payload)
      .pipe(catchError(this.handleError));
  }

  /**
   * Approve or reject a booking.
   * @param payload { bookingId, status ("Approved"|"Rejected"), adminRemarks }
   * @param adminId Admin ID to pass as query param
   */
  approveOrRejectRequest(
    payload: { bookingId: number; status: string; adminRemarks: string },
    adminId: number
  ): Observable<void> {
    return this.http.post<void>(
      `${this.apiUrl}/approve-reject?adminId=${adminId}`,
      payload
    ).pipe(catchError(this.handleError));
  }

  /**
   * Generic error handler for all HTTP requests.
   */
  private handleError(error: HttpErrorResponse) {
    console.error('Backend returned an error:', error);
    return throwError(() => new Error('Unable to process the request. Please try again later.'));
  }
}