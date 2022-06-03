import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { bookDetails } from '../models/bookDetails';
import { issueDetails } from '../models/issueDetails';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  readonly APIUrl = 'https://localhost:7298/api';

  constructor(private http: HttpClient) { }

  getBookList(): Observable<bookDetails[]> {
    return this.http.get<any>(this.APIUrl + '/Book');
  }

  addBook(details: bookDetails) {
    return this.http.post(this.APIUrl + '/Book', details);
  }

  updateBook(id: number, details: bookDetails) {
    return this.http.put(this.APIUrl + `/Book/${id}`, details);
  }

  deleteBook(id: number) {
    return this.http.delete(this.APIUrl + `/Book/${id}`);
  }

  requestBook(details: issueDetails) {
    return this.http.post(this.APIUrl + '/Issue/request', details);
  }

  updateApproveStatus(id: number, details: issueDetails) {
    return this.http.put(this.APIUrl + `/Approve/${id}`, details);
  }

  getBooksToApprove(): Observable<issueDetails[]> {
    return this.http.get<any>(this.APIUrl + '/Approve');
  }

  getIssueDetails(): Observable<issueDetails[]> {
    return this.http.get<any>(this.APIUrl + '/Issue');
  }

  getStudentBooks(): Observable<issueDetails[]> {
    return this.http.get<any>(this.APIUrl + '/Submission');
  }

  updateReturnStatus(id: number, details: issueDetails) {
    return this.http.put(this.APIUrl + `/Submission/${id}`, details);
  }
}
