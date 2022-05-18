import { Component, Input, OnInit } from '@angular/core';
import { BookService } from '../../service/book.service';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-add-edit-books',
  templateUrl: './add-edit-books.component.html',
  styleUrls: ['./add-edit-books.component.css']
})
export class AddEditBooksAdminComponent implements OnInit {
  constructor(private service: SharedService, private bookService: BookService) { }

  @Input() book: any;
  BookId: string = '';
  BookName: string = '';
  BookAuthor: string = '';
  BookQuantity: string = '';

  ngOnInit(): void {
    this.BookId = this.book.bId;
    this.BookName = this.book.bName;
    this.BookAuthor = this.book.bAuthor;
    this.BookQuantity = this.book.bQuantity;
  }

  addBook() {
    var details = {
      bName: this.BookName,
      bAuthor: this.BookAuthor,
      bQuantity: this.BookQuantity
    };
    console.log;

    this.bookService.addBook(details).subscribe((res) => {
      this.service.SnackBarMessage(JSON.stringify(res), "Dismiss");
    });
  }

  updateBook() {
    if (Number(this.BookQuantity) < 1) {
      this.service.SnackBarMessage("Quantity must be more than 0", "Dismiss");
    }
    else {
      var details = { bId: this.BookId, bName: this.BookName, bAuthor: this.BookAuthor, bQuantity: this.BookQuantity };
      this.bookService.updateBook(this.BookId, details).subscribe((res) => {
        this.service.SnackBarMessage(JSON.stringify(res), "Dismiss");
      });
    }
  }
}
