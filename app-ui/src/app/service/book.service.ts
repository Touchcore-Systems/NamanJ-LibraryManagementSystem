import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  readonly APIUrl = 'https://localhost:7298/api';

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }

  getBookList(): Observable<any[]> {
    return this.http.get<any>(this.APIUrl + '/Book');
  }

  addBook(details: any) {
    return this.http.post(this.APIUrl + '/Book', details);
  }

  updateBook(id: any, details: any) {
    return this.http.put(this.APIUrl + `/Book/${id}`, details);
  }

  deleteBook(id: any) {
    return this.http.delete(this.APIUrl + `/Book/${id}`);
  }

  requestBook(details: any){
    console.log(details);
    return this.http.post(this.APIUrl + '/Issue/request', details);
  }

  updateApproveStatus(id: any, details: any){
    return this.http.put(this.APIUrl + `/Approve/${id}`, details);
  }

  getBooksToApprove(): Observable<any[]>{
    return this.http.get<any>(this.APIUrl + '/Approve');
  }

  getIssueDetails(): Observable<any[]>{
    return this.http.get<any>(this.APIUrl + '/Issue');
  }

  getStudentBooks(): Observable<any[]>{
    return this.http.get<any>(this.APIUrl + '/Submission');
  }

  updateReturnStatus(id: any, details: any){
    return this.http.put(this.APIUrl + `/Submission/${id}`, details);
  }
}
