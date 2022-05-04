import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SharedService } from '../../shared.service';

@Component({
  selector: 'app-issue-details',
  templateUrl: './issue-details.component.html',
  styleUrls: ['./issue-details.component.css']
})
export class IssueDetailsComponent implements OnInit {

  constructor(private service: SharedService) { }

  ngOnInit(): void {
    this.refreshIssueList();
  }

  @ViewChild('paginator') paginator!: MatPaginator;
  @ViewChild(MatSort) matSort! : MatSort;

  IssueList!: MatTableDataSource<any>;
  issueDetail: string[] = ['t_id', 'u_name', 'b_name', 'date_of_issue', 'date_of_submission', 'date_of_return', 'status', 'fine'];
  isEmpty = false;

  refreshIssueList() {
    this.service.getIssueDetails().subscribe((data) => {
      if(data.length == 0){
        this.isEmpty = true;
      }
      this.IssueList = new MatTableDataSource(data);
      this.IssueList.paginator = this.paginator;
      this.IssueList.sort = this.matSort;
    });
  }

  filterData($event: any){
    this.IssueList.filter = $event.target.value;
  }
}
