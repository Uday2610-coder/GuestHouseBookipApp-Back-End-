import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Material modules
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatRadioModule } from '@angular/material/radio';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatStepperModule } from '@angular/material/stepper';
import { MatCardModule } from '@angular/material/card';
//Components
import { AdminNavComponent } from '../shared/admin-nav/admin-nav.component';
import { AdminDashboardComponent } from './dashboard/dashboard.component';
import { AdminReservationListComponent } from './reservation-list/reservation-list.component';
import { PendingRequestComponent } from './pending-request/pending-request.component';
import { ApproveRejectFormComponent } from './approve-reject-form/approve-reject-form.component';
import { AdminReportComponent } from './report/report.component';
import { GuestHouseMasterComponent } from './guest-house-master/guest-house-master.component';

// Routing
import { AdminRoutingModule } from './admin-routing.module';
import { UpdateBookingFormComponent } from './update-booking-form/update-booking-form.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';

@NgModule({
  declarations: [
    AdminNavComponent,
    AdminDashboardComponent,
    AdminReservationListComponent,
    PendingRequestComponent,
    ApproveRejectFormComponent,
    AdminReportComponent,
    GuestHouseMasterComponent,
    UpdateBookingFormComponent,
    ConfirmDialogComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    AdminRoutingModule,
    MatButtonModule,
    MatCardModule,
    MatRadioModule,
    MatDialogModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatSelectModule,
    MatOptionModule,
    MatSlideToggleModule,
    MatChipsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatStepperModule
  ],
  exports: [
    AdminNavComponent,
    PendingRequestComponent
  ]
})
export class AdminModule {}