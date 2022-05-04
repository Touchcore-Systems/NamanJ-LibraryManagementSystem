import { Component, OnInit, ViewChild } from '@angular/core';
import { SharedService } from '../../shared.service';
import { MatTableDataSource } from '@angular/material/table'
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-show-book',
  templateUrl: './show-book.component.html',
  styleUrls: ['./show-book.component.css'],
})
export class ShowBookAdminComponent implements OnInit {

  constructor(private service: SharedService) {  }

  BookList!: MatTableDataSource<any>;
  availableBooks: string[] = ['b_id', 'b_name', 'b_author', 'b_quantity', 'Edit', 'Delete'];

  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) matSort! : MatSort;

  ModalTitle: string = '';
  ActivateAddEditBookComp: boolean = false;
  book: any;
  isEmpty = false;

  ngOnInit(): void {
    this.refreshBookList();
  }

  addClick() {
    this.book = {
      b_id: 0,
      b_name: '',
      b_author: '',
      b_quantity: 0
    };
    this.ModalTitle = 'Add Book';
    this.ActivateAddEditBookComp = true;
  }

  closeClick() {
    this.ActivateAddEditBookComp = false;
    this.refreshBookList();
  }

  editClick(item: any) {
    this.book = item;
    this.ModalTitle = 'Edit Book';
    this.ActivateAddEditBookComp = true;
  }

  deleteClick(item: any) {
    if (confirm('Are you sure?')) {
      this.service.deleteBook(item.b_id).subscribe(data => {
        // alert(data.toString());
        this.service.SnackBarMessage(data.toString(), "Dismiss");
        this.refreshBookList();
      })
    }
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
