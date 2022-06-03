import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { MatSnackBar } from '@angular/material/snack-bar';

import { users } from './models/users';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  readonly APIUrl = 'https://localhost:7298/api';

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService, private SnackBar: MatSnackBar) { }

  loginUser(credentials: users) {
    return this.http.post(this.APIUrl + '/Authentication/login', credentials);
  }

  registerUser(details: users) {
    return this.http.post(this.APIUrl + '/Users/register', details);
  }

  isUserAuthenticated() {
    const token: any = localStorage.getItem("lms_authenticate");

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    } else {
      return false;
    }
  }

  logOut() {
    localStorage.removeItem("lms_authenticate");
  }

  SnackBarSuccessMessage(message: string) {
    this.SnackBar.open(message, "Dismiss", {
      duration: 3000,
      verticalPosition: 'top',
      panelClass: ['text-light'],
    });
  }

  SnackBarErrorMessage(message: string) {
    this.SnackBar.open(message, "Dismiss", {
      duration: 3000,
      verticalPosition: 'top',
      panelClass: ['text-danger'],
    });
  }
}
