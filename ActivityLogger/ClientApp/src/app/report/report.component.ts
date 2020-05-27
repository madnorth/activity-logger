import { Component, OnInit, ViewChild } from '@angular/core';
import { ReportItem } from './shared/report-item.model';
import { ActivityService } from '../activities/shared/activity.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})
export class ReportComponent implements OnInit {

  dataSource: MatTableDataSource<ReportItem>;
  displayedColumns: string[] = ['categoryName', 'duration', 'comments'];

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private activityService: ActivityService
  ) { }

  ngOnInit(): void {
    this.fetchData();
  }

  private fetchData(): void {
    this.activityService.fetchReportData("2020-05-24")
      .subscribe(
        data => {
          this.dataSource = new MatTableDataSource(data);
          this.dataSource.sort = this.sort;
        },
        error => {
          //this.snackBar.open(error, "close");
        }
      );
  }
}
