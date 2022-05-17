import { Component, OnInit, ViewChild } from '@angular/core';
import { SharedService } from '../../shared.service';
import { MatTableDataSource } from '@angular/material/table'
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { BookService } from '../../service/book.service';

@Component({
  selector: 'app-show-books',
  templateUrl: './show-books.component.html',
  styleUrls: ['./show-books.component.css'],
})
export class ShowBooksAdminComponent implements OnInit {

  constructor(private service: SharedService, private bookService: BookService) { }

  BookList!: MatTableDataSource<any>;
  availableBooks: string[] = ['bId', 'bName', 'bAuthor', 'bQuantity', 'Edit', 'Delete'];

  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) matSort!: MatSort;

  ModalTitle: string = '';
  ActivateAddEditBookComp: boolean = false;
  book: any;
  isEmpty = false;

  ngOnInit(): void {
    this.refreshBookList();
  }

  addClick() {
    this.book = {
      bId: 0,
      bName: '',
      bAuthor: '',
      bQuantity: 0
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
      this.bookService.deleteBook(item.bId).subscribe(data => {
        // alert(data.toString());
        this.service.SnackBarMessage(data.toString(), "Dismiss");
        this.refreshBookList();
      })
    }
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
