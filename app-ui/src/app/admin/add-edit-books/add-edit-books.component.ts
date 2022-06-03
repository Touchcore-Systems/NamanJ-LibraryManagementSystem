import { Component, Input, OnInit } from '@angular/core';
import { bookDetails } from 'src/app/models/bookDetails';
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
    var details: bookDetails = {
      bName: this.BookName,
      bAuthor: this.BookAuthor,
      bQuantity: parseInt(this.BookQuantity)
    };

    try {
      this.bookService.addBook(details).subscribe((res) => {
        this.service.SnackBarSuccessMessage(JSON.stringify(res));
      }, err => {
        this.service.SnackBarErrorMessage(JSON.stringify(err.message));
      });
    } catch (error: any) {
      this.service.SnackBarErrorMessage(JSON.stringify(error));
    }
  }

  updateBook() {
    if (Number(this.BookQuantity) < 1) {
      this.service.SnackBarErrorMessage("Quantity must be more than 0");
    }
    else {
      var details: bookDetails = {
        bId: parseInt(this.BookId),
        bName: this.BookName,
        bAuthor: this.BookAuthor,
        bQuantity: parseInt(this.BookQuantity)
      };
      try {
        this.bookService.updateBook(parseInt(this.BookId), details).subscribe((res) => {
          this.service.SnackBarSuccessMessage(JSON.stringify(res));
        }, err => {
          this.service.SnackBarErrorMessage(JSON.stringify(err.message));
        });
      } catch (error: any) {
        this.service.SnackBarErrorMessage(JSON.stringify(error));
      }
    }
  }
}
