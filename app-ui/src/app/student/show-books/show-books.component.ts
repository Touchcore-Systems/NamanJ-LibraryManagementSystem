import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BookService } from '../../service/book.service';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-show-book',
  templateUrl: './show-books.component.html',
  styleUrls: ['./show-books.component.css']
})
export class ShowBooksStudentComponent implements OnInit {

  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) matSort!: MatSort;

  BookList!: MatTableDataSource<any>;
  availableBooks: string[] = ['bId', 'bName', 'bAuthor', 'bQuantity', 'Request'];
  book: any;
  isEmpty = false;

  uName = JSON.parse(window.atob(localStorage.getItem('lms_authenticate')!.split('.')[1]))["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];

  constructor(private service: SharedService, private bookService: BookService) { }

  ngOnInit(): void {
    this.refreshBookList();
  }

  requestBook(item: any) {
    this.book = item;

    if (this.book.bQuantity >= 1) {
      var details = {
        uName: this.uName,
        bId: this.book.bId,
        Status: 'pending'
      };

      this.bookService.requestBook(details).subscribe((res) => {
        this.service.SnackBarMessage(res.toString(), "Dismiss");
      });
    } else {
      this.service.SnackBarMessage("Out of stock!", "Dismiss")
    }
    this.refreshBookList();
  }

  refreshBookList() {
    this.bookService.getBookList().subscribe((data) => {
      if (data.length == 0) {
        this.isEmpty = true;
      }
      this.BookList = new MatTableDataSource(data);
      this.BookList.paginator = this.paginator;
      this.BookList.sort = this.matSort;
    });
  }

  filterData($event: any) {
    this.BookList.filter = $event.target.value;
  }
}
