import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

import { ReportItem } from './shared/report-item.model';
import { ActivityService } from '../activities/shared/activity.service';

import * as moment from 'moment';

const DATE_FORMAT: string = 'YYYY-M-D';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})
export class ReportComponent implements OnInit, OnDestroy {

  dataSource: MatTableDataSource<ReportItem>;
  displayedColumns: string[] = ['categoryName', 'duration', 'comments'];
  selectedDate: moment.Moment;
  queriedDate: moment.Moment;

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  private fetchData$: Subscription;

  constructor(
    private activityService: ActivityService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.fetchData(moment().format(DATE_FORMAT));
  }

  ngOnDestroy(): void {
    if (this.fetchData$) {
      this.fetchData$.unsubscribe();
    }

    this.snackBar.dismiss();
  }

  onGetReport(): void {
    this.fetchData(this.selectedDate.format(DATE_FORMAT));
    this.dataSource = null;
    this.selectedDate = undefined;
  }

  private fetchData(date: string): void {
    this.fetchData$ = this.activityService.fetchReportData(date)
      .subscribe(
        respData => {
          this.dataSource = new MatTableDataSource(respData.data);
          this.dataSource.sort = this.sort;
          this.queriedDate = moment(respData.queriedDate);
        },
        error => {
          this.snackBar.open(error, "close");
        }
      );
  }
}
