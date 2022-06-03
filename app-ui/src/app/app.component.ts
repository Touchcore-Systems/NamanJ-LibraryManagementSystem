import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from './shared.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  u_role: any = "";
  constructor(private router: Router, private service: SharedService) { }

  ngOnInit(): void {
    this.isAuthenticated()
    this.isAdmin();
    this.isStudent();
  }

  isAuthenticated() {
    if (this.service.isUserAuthenticated()) {
      this.u_role = JSON.parse(window.atob(localStorage.getItem('lms_authenticate')!.split('.')[1]))["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      return true;
    } else {
      return false;
    }
  }

  isAdmin() {
    if (this.u_role == 'admin') {
      return true;
    } else {
      return false;
    }
  }

  isStudent() {
    if (this.u_role == 'student') {
      return true;
    } else {
      return false;
    }
  }

  logout() {
    this.service.logOut();
  }
}
