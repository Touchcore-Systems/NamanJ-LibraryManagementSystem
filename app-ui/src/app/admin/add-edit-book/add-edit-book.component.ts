import { Component, Input, OnInit } from '@angular/core';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-add-edit-book',
  templateUrl: './add-edit-book.component.html',
  styleUrls: ['./add-edit-book.component.css'],
})
export class AddEditBookComponent implements OnInit {
  constructor(private service: SharedService) { }

  @Input() book: any;
  BookId: string = '';
  BookName: string = '';
  BookAuthor: string = '';
  BookQuantity: string = '';

  ngOnInit(): void {
    this.BookId = this.book.b_id;
    this.BookName = this.book.b_name;
    this.BookAuthor = this.book.b_author;
    this.BookQuantity = this.book.b_quantity;
  }

  addBook() {
    var val = {
      BookId: this.BookId,
      b_name: this.BookName,
      b_author: this.BookAuthor,
      b_quantity: this.BookQuantity
    };
    console.log;

    this.service.addBook(val).subscribe((res) => {
      // alert(res.toString());
      this.service.SnackBarMessage(res.toString(), "Dismiss");
    });
  }

  updateBook() {
    if(Number(this.BookQuantity) < 1){
      this.service.SnackBarMessage("Quantity must be more than 0", "Dismiss");
    }
    else{
      var val = { b_id: this.BookId, b_name: this.BookName, b_author: this.BookAuthor, b_quantity: this.BookQuantity };
      this.service.updateBook(val).subscribe((res) => {
      // alert(res.toString());
      this.service.SnackBarMessage(res.toString(), "Dismiss");
    });
    }
  }
}
