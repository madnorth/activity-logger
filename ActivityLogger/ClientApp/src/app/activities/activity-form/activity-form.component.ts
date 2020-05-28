import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';

import * as moment from 'moment';
import { Category } from '../shared/category.model';
import { ActivityService } from '../shared/activity.service';

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
export class ActivityFormComponent implements OnInit, OnDestroy {

  activityForm: FormGroup;
  categories: Category[];
  isSaving = false;
  nowDateTime: moment.Moment;

  private fetchCategories$: Subscription;
  private saveActivity$: Subscription;

  constructor(
    private fb: FormBuilder,
    private activityService: ActivityService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.nowDateTime = moment();
    this.activityForm = this.fb.group({
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

    this.fetchCategories();
  }

  ngOnDestroy(): void {
    if (this.fetchCategories$) {
      this.fetchCategories$.unsubscribe();
    }

    if (this.saveActivity$) {
      this.saveActivity$.unsubscribe();
    }

    this.snackBar.dismiss();
  }

  onSubmit(): void {
    this.isSaving = true;
    this.saveActivity();
  }

  private fetchCategories(): void {
    this.fetchCategories$ = this.activityService.getAvailableCategories()
      .subscribe(
        data => {
          this.categories = data
        },
        error => {
          this.snackBar.open(error, "close");
        }
      );
  }

  private retrieveDateTimeString(date: moment.Moment, timeString: string): string {
    const time = this.retrieveTime(timeString);
    date.hour(time.hour);
    date.minute(time.minute);
    date.second(0);
    return date.format();
  }

  private retrieveTime(timeString: string): Time {
    var temp = timeString.split(":");
    return new Time(Number(temp[0]), Number(temp[1]));
  }

  private saveActivity(): void {
    this.saveActivity$ = this.activityService.createNewActivity(
      this.activityForm.value['category'],
      this.retrieveDateTimeString(this.activityForm.get('startDateTime.startDate').value, this.activityForm.get('startDateTime.startTime').value),
      this.retrieveDateTimeString(this.activityForm.get('endDateTime.endDate').value, this.activityForm.get('endDateTime.endTime').value),
      this.activityForm.value['comment']
    )
      .subscribe(
        data => {
          this.activityService.addActivity(data);
          this.activityForm.reset();
          this.snackBar.open("Successful saving.", "close");
          this.isSaving = false;
        },
        error => {
          this.isSaving = false;
          this.snackBar.open(error, "close");
        }
      );
  }
}
