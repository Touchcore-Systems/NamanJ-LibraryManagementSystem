import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-books-to-approve',
  templateUrl: './books-to-approve.component.html',
  styleUrls: ['./books-to-approve.component.css']
})
export class BooksToApproveComponent implements OnInit {

  constructor(private service: SharedService) { }

  ngOnInit(): void {
    this.refreshToApproveList();
  }

  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) matSort! : MatSort;

  ApproveList!: MatTableDataSource<any>;
  approveDetail: string[] = ['t_id', 'u_name', 'b_id', 'b_name', 'b_author', 'Approve'];
  isEmpty = false;

  approveBook(item: any) {
    if (item.b_quantity >= 1) {
      var val = { t_id: item.t_id };
      console.log(val);
      this.service.updateApproveStatus(val).subscribe((res) => {
        this.service.SnackBarMessage(res.toString(), "Dismiss");
      });
      var q_val = { b_id: item.b_id, b_name: item.b_name, b_author: item.b_author, b_quantity: (item.b_quantity - 1) };
      this.service.updateBook(q_val).subscribe((res) => {

      });
      this.refreshToApproveList();
    } else {
      this.service.SnackBarMessage("Out of stock!", "Dismiss");
    }
  }

  refreshToApproveList() {
    this.service.getToApprove().subscribe((data) => {
      if(data.length == 0){
        this.isEmpty = true;
      }
      this.ApproveList = new MatTableDataSource(data);
      this.ApproveList.paginator = this.paginator;
      this.ApproveList.sort = this.matSort;
    });
  }

  filterData($event: any){
    this.ApproveList.filter = $event.target.value;
  }
}
