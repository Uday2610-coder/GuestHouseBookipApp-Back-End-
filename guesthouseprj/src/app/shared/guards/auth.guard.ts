import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const token = sessionStorage.getItem('authToken');
    if (!token) {
      console.error('No token found. Redirecting to login.');
      this.router.navigate(['/login']);
      return false;
    }

    try {
      const decodedToken: any = JSON.parse(atob(token.split('.')[1])); // Decode JWT payload
      console.log('Decoded Token:', decodedToken);

      const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds
      if (decodedToken.exp < currentTime) {
        console.error('Token has expired. Redirecting to login.');
        this.router.navigate(['/login']);
        return false;
      }

      const userRole = decodedToken.role;
      console.log('Decoded Role:', userRole);

      if (route.data['role'] && route.data['role'] !== userRole) {
        console.error('Role mismatch. Redirecting to login.');
        this.router.navigate(['/login']);
        return false;
      }

      console.log('Access granted for role:', userRole);
      return true; // Allow access
    } catch (error) {
      console.error('Error decoding token:', error);
      this.router.navigate(['/login']);
      return false;
    }
  }
}