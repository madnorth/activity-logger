import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

import * as _moment from 'moment';
import { Category } from '../shared/category.model';
import { ActivityService } from '../shared/activity.service';
import { MatSnackBar } from '@angular/material/snack-bar';
const moment = _moment;

class Time {
  constructor(
    public hour: number,
    public minute: number
  ) { }
}

@Component({
  selector: 'app-activity-form',
  templateUrl: './activity-form.component.html',
  styleUrls: ['./activity-form.component.scss']
})
export class ActivityFormComponent implements OnInit {

  nowDateTime = moment();
  categories: Category[] = [];

  activityForm = this.fb.group({
    category: ['', Validators.required],
    startDateTime: this.fb.group({
      startDate: [this.nowDateTime, Validators.required],
      startTime: ['', Validators.required]
    }),
    endDateTime: this.fb.group({
      endDate: [this.nowDateTime, Validators.required],
      endTime: [this.nowDateTime.hour() + ':' + this.nowDateTime.minute(), Validators.required]
    }),
    comment: ['', Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private activityService: ActivityService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.fetchCategories();
  }

  onSubmit(): void {
    console.log(this.activityForm.value);
    this.activityForm.reset();
  }

  fetchCategories(): void {
    this.activityService.getAvailableCategories()
      .subscribe(
        data => {
          this.categories = data
        },
        error => {
          this.snackBar.open(error, "close");
        }
      );
  }

  retrieveTime(timeString: string): Time {
    var temp = timeString.split(":");
    return new Time(Number(temp[0]), Number(temp[1]));
  }
}
