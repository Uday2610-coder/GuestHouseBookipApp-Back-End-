import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { BookingPageComponent } from './booking-page/booking-page.component';
import { MybookingsComponent } from './mybookings/mybookings.component';
import { UserLayoutComponent } from './user-layout/user-layout.component';

const routes: Routes = [
  {
    path: '',
    component: UserLayoutComponent,
    children: [
      { path: 'bookings', component: BookingPageComponent },
      { path: 'my-bookings', component: MybookingsComponent },
      { path: '', redirectTo: 'bookings', pathMatch: 'full' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }