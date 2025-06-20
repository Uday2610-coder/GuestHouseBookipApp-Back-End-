import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  forgotForm: FormGroup;
  message: string = '';
  error: string = '';

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit() {
  if (this.forgotForm.invalid) return;
  const email = this.forgotForm.value.email;
  this.authService.forgotPassword(email).subscribe({
    next: () => {
      this.message = 'If your email is registered, a reset link has been sent.';
      this.error = '';
    },
    error: (err) => {
      console.error('Forgot password error:', err); // Debug: log error
      // Try to display backend error if available
      if (err?.error) {
        this.error = typeof err.error === 'string'
          ? err.error
          : (err.error.message || 'Error sending reset link.');
      } else {
        this.error = 'Error sending reset link.';
      }
      this.message = '';
    }
  });
}
}