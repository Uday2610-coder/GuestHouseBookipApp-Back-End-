import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../services/auth.service';
import { jwtDecode } from 'jwt-decode'; // Use named import for jwtDecode

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  // Initialize the login form
  private initializeForm(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  // Login Functionality
  onLogin(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;
      const credentials = this.loginForm.value;

      this.authService.login(credentials).subscribe({
        next: (response) => {
          this.isLoading = false;

          // Save token to sessionStorage
          sessionStorage.setItem('authToken', response.token);
          console.log('Backend Response:', response);

          try {
            // Decode the token to extract the role
            const decodedToken: any = jwtDecode(response.token); // Use named import for jwtDecode
            const userRole = decodedToken.role;

            // Save the role to sessionStorage
            sessionStorage.setItem('userRole', userRole);

            this.snackBar.open('Login successful!', 'Close', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
              panelClass: ['success-snackbar'],
            });

            // Redirect based on user role
            if (userRole === 'Admin') {
              console.log('Redirecting to admin dashboard');
              this.router.navigate(['/admin/dashboard']);
            } else if (userRole === 'User') {
              console.log('Redirecting to user bookings');
              this.router.navigate(['/user/bookings']);
            } else {
              this.snackBar.open(
                'Invalid role. Please contact support.',
                'Close',
                {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                  panelClass: ['error-snackbar'],
                }
              );
            }
          } catch (error) {
            console.error('Error decoding token:', error);
            this.snackBar.open(
              'Invalid token. Please try again.',
              'Close',
              {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
                panelClass: ['error-snackbar'],
              }
            );
          }
        },
        error: (error) => {
          this.isLoading = false;
          console.error('Login error:', error);
          this.snackBar.open(
            error.error?.message || 'Invalid credentials. Please try again.',
            'Close',
            {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
              panelClass: ['error-snackbar'],
            }
          );
        },
      });
    }
  }
}