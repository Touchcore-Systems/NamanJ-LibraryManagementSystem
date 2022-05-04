import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-show-book',
  templateUrl: './show-book.component.html',
  styleUrls: ['./show-book.component.css']
})
export class ShowBookStudentComponent implements OnInit {

  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) matSort! : MatSort;

  BookList!: MatTableDataSource<any>;
  availableBooks: string[] = ['b_id', 'b_name', 'b_author', 'b_quantity', 'Request'];
  book: any;
  isEmpty = false;

  u_name = JSON.parse(window.atob(localStorage.getItem('lms_authenticate')!.split('.')[1]))["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];

  constructor(private service: SharedService) { }

  ngOnInit(): void {
    this.refreshBookList();
  }

  requestBook(item: any) {
    this.book = item;

    if (this.book.b_quantity >= 1) {
      var val = {
        u_name: this.u_name,
        b_id: this.book.b_id,
        b_name: this.book.b_name,
        b_author: this.book.b_author,
        status: 'pending'
      };

      console.log(val.u_name)
      this.service.requestBook(val).subscribe((res) => {
        // alert(res.toString());
        this.service.SnackBarMessage(res.toString(), "Dismiss");
      });
    } else {
      this.service.SnackBarMessage("Out of stock!", "Dismiss")
      // alert("Out of stock!");
    }
    this.refreshBookList();
  }

  refreshBookList() {
    this.service.getBookList().subscribe((data) => {
      if(data.length == 0){
        this.isEmpty = true;
      }
      this.BookList = new MatTableDataSource(data);
      this.BookList.paginator = this.paginator;
      this.BookList.sort = this.matSort;
    });
  }

  filterData($event: any){
    this.BookList.filter = $event.target.value;
  }
}
