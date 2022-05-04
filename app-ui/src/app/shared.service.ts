import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
// handle asynchronous requests
import { Observable } from 'rxjs';
import {MatSnackBar} from '@angular/material/snack-bar';


@Injectable({
  providedIn: 'root'
})
export class SharedService {

  readonly APIUrl = 'https://localhost:7248/api';
  
  constructor(private http: HttpClient, private jwtHelper: JwtHelperService, private SnackBar: MatSnackBar) { }

  getBookList(): Observable<any[]>{
    return this.http.get<any>(this.APIUrl + '/Book');
  }

  addBook(val: any) {
    return this.http.post(this.APIUrl + '/Book', val);
  }

  updateBook(val: any){
    return this.http.put(this.APIUrl + '/Book', val);
  }

  deleteBook(val: any){
    return this.http.delete(this.APIUrl + '/Book/' + val);
  }

  requestBook(val: any){
    return this.http.post(this.APIUrl + '/Issue', val);
  }

  registerUser(val: any){
    return this.http.post(this.APIUrl + '/Register', val);
  }

  isUserAuthenticated() {
    const token: any = localStorage.getItem("lms_authenticate");

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    } else {
      return false;
    }
  }

  getIssueDetails(): Observable<any[]>{
    return this.http.get<any>(this.APIUrl + '/Issue');
  }

  getToApprove(): Observable<any[]>{
    return this.http.get<any>(this.APIUrl + '/Approve');
  }

  updateApproveStatus(val: any){
    return this.http.put(this.APIUrl + '/Approve', val);
  }

  getStudentBooks(): Observable<any[]>{
    return this.http.get<any>(this.APIUrl + '/Submission');
  }

  updateReturnStatus(val: any){
    return this.http.put(this.APIUrl + '/Submission', val);
  }

  logOut(){
    localStorage.removeItem("lms_authenticate");
  }

  SnackBarMessage(message: string, action: string) {
    this.SnackBar.open(message, action, {
      duration: 3000,
    });
  }
}
