import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-my-books',
  templateUrl: './my-books.component.html',
  styleUrls: ['./my-books.component.css']
})
export class MyBooksComponent implements OnInit {

  constructor(private service: SharedService) { }

  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) matSort! : MatSort;

  StudentBooks!: MatTableDataSource<any>;
  stBooks: string[] = ['t_id', 'b_name', 'b_author', 'date_of_submission', 'Action'];
  isEmpty = false;

  ngOnInit(): void {
    this.refreshStudentBooks();
  }

  
  returnBook(item: any) {
    var diff = this.calculateDiff(item.date_of_submission);
    if (diff >= 1) {
      var returnFine: any = returnFine * 10;
    } else {
      returnFine = 0;
    }

    var val = {
      t_id: item.t_id,
      date_of_return: new Date(),
      fine: returnFine
    };

    this.service.updateReturnStatus(val).subscribe((res) => {
      // alert(res);
      this.service.SnackBarMessage(res.toString(), "Dismiss")
    });

    var q_val = { b_id: item.b_id, b_name: item.b_name, b_author: item.b_author, b_quantity: (item.b_quantity + 1) };
    this.service.updateBook(q_val).subscribe((res) => {});

    this.refreshStudentBooks();
  }

  calculateDiff(data: any) {
    let date = new Date(data);
    let currentDate = new Date();

    let days = Math.floor((currentDate.getTime() - date.getTime()) / 1000 / 60 / 60 / 24);
    return days;
  }

  refreshStudentBooks() {
    this.service.getStudentBooks().subscribe((data) => {
      if(data.length == 0){
        this.isEmpty = true;
      }
      this.StudentBooks = new MatTableDataSource(data);
      this.StudentBooks.paginator = this.paginator;
      this.StudentBooks.sort = this.matSort;
    });
  }

  filterData($event: any){
    this.StudentBooks.filter = $event.target.value;
  }
}
