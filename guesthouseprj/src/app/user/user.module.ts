import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


// Material Imports
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox'; // <-- ADD THIS LINE

import { MatDialogModule } from '@angular/material/dialog';


// Components
import { UserNavComponent } from './user-nav/user-nav.component';
import { BookingPageComponent } from './booking-page/booking-page.component';
import { MybookingsComponent } from './mybookings/mybookings.component';

// Routing
import { UserRoutingModule } from './user-routing.module';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { UserHeaderComponent } from './user-header/user-header.component';
import { UserFooterComponent } from './user-footer/user-footer.component';
import { UserLayoutComponent } from './user-layout/user-layout.component';

@NgModule({
  declarations: [
    UserNavComponent,
    BookingPageComponent,
    MybookingsComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    UserHeaderComponent,
    UserFooterComponent,
    UserLayoutComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    UserRoutingModule,
    
    // Material Modules
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatChipsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule
  ],
  exports: [
    UserNavComponent,
     ForgotPasswordComponent,  
    ResetPasswordComponent 
  ]
})
export class UserModule { } 