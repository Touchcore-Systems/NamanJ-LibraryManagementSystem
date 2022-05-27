import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BookService } from '../../service/book.service';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-my-books',
  templateUrl: './my-books.component.html',
  styleUrls: ['./my-books.component.css']
})
export class MyBooksComponent implements OnInit {

  constructor(private service: SharedService, private bookService: BookService) { }

  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) matSort!: MatSort;

  StudentBooks!: MatTableDataSource<any>;
  stBooks: string[] = ['tId', 'bName', 'bAuthor', 'DateOfSubmission', 'Action'];
  isEmpty = false;

  ngOnInit(): void {
    this.refreshStudentBooks();
  }

  returnBook(item: any) {
    var diff = this.calculateDiff(item.DateOfSubmission);
    if (diff >= 1) {
      var returnFine: any = diff * 10;
    } else {
      returnFine = 0;
    }

    var details = {
      DateOfReturn: new Date(),
      fine: returnFine
    };

    this.bookService.updateReturnStatus(item.tId, details).subscribe((res) => {
      this.service.SnackBarMessage(JSON.stringify(res), "Dismiss");
      this.refreshStudentBooks();
    });
  }

  calculateDiff(data: any) {
    let date = new Date(data);
    let currentDate = new Date();

    let days = Math.floor((currentDate.getTime() - date.getTime()) / 1000 / 60 / 60 / 24);
    return days;
  }

  refreshStudentBooks() {
    this.bookService.getStudentBooks().subscribe((data) => {
      data.length == 0 ? this.isEmpty = true : this.isEmpty = false;
      this.StudentBooks = new MatTableDataSource(data);
      this.StudentBooks.paginator = this.paginator;
      this.StudentBooks.sort = this.matSort;
    });
  }

  filterData($event: any) {
    this.StudentBooks.filter = $event.target.value;
  }
}
