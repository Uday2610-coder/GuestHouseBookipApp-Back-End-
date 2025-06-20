import { Component,  OnInit } from '@angular/core';

@Component({
  selector: 'app-user-layout',
  templateUrl: './user-layout.component.html',
  styleUrls: ['./user-layout.component.css']
})
export class UserLayoutComponent {
 username: string = 'User';

  ngOnInit(): void {
    // If you have a user service, fetch username here.
    // Example:
    // this.username = this.authService.getCurrentUser().name;
  }
}
