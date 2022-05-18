import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BookService } from '../../service/book.service';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-books-to-approve',
  templateUrl: './books-to-approve.component.html',
  styleUrls: ['./books-to-approve.component.css']
})
export class BooksToApproveComponent implements OnInit {

  constructor(private service: SharedService, private bookService: BookService) { }

  ngOnInit(): void {
    this.refreshToApproveList();
  }

  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) matSort!: MatSort;

  ApproveList!: MatTableDataSource<any>;
  approveDetail: string[] = ['tId', 'uName', 'bId', 'bName', 'bAuthor', 'Approve'];
  isEmpty = false;

  approveBook(item: any) {
    console.log(item);
    var approveStatus = { Status: item.Status}
    if (item.bQuantity >= 1) {
      this.bookService.updateApproveStatus(item.tId, approveStatus).subscribe((res) => {
        this.service.SnackBarMessage(JSON.stringify(res), "Dismiss");
        this.refreshToApproveList();
      });
    } else {
      this.service.SnackBarMessage("Out of stock!", "Dismiss");
    }
  }

  refreshToApproveList() {
    this.bookService.getBooksToApprove().subscribe((data) => {
      data.length == 0 ? this.isEmpty = true : this.isEmpty = false;
      this.ApproveList = new MatTableDataSource(data);
      this.ApproveList.paginator = this.paginator;
      this.ApproveList.sort = this.matSort;
    });
  }

  filterData($event: any) {
    this.ApproveList.filter = $event.target.detailsue;
  }
}
