import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5003/api/User'; // Replace with your backend API URL

  constructor(private http: HttpClient) {}

  /**
   * Sends a login request to the backend.
   * @param credentials - An object containing email and password.
   * @returns An observable of the login response.
   */
  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials);
  }

  /**
   * Logs out the user by clearing the session.
   */
  logout(): void {
    sessionStorage.removeItem('authToken');
    sessionStorage.removeItem('userRole');
  }

  /**
   * Checks if the user is logged in.
   * @returns True if the user is logged in, false otherwise.
   */
  isLoggedIn(): boolean {
    const token = sessionStorage.getItem('authToken');
    return !!token;
  }
    forgotPassword(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/forgot-password`, { email });
  }
   resetPassword(email: string, token: string, newPassword: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/reset-password`, { email, token, newPassword });
  }

  getCurrentUserId(): number | null {
    const token = sessionStorage.getItem('authToken');
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['nameid'] ? +payload['nameid'] : null;
    } catch (e) {
      return null;
    }
  }
}