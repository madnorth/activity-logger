import { Component, OnInit, OnDestroy } from '@angular/core';
import { Activity } from '../shared/activity.model';
import { ActivityService } from '../shared/activity.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-activity-list',
  templateUrl: './activity-list.component.html',
  styleUrls: ['./activity-list.component.scss']
})
export class ActivityListComponent implements OnInit, OnDestroy {

  activities: Activity[];
  isLoading = false;

  private activitiesChange$: Subscription;

  constructor(
    private activityService: ActivityService
  ) { }

  ngOnInit(): void {
    this.fetchActivities();
    this.activities = this.activityService.getActivities();
    this.activitiesChange$ = this.activityService.activitiesChanged
      .subscribe(
        data => {
          this.activities = data;
        }
      );
  }

  ngOnDestroy(): void {
    this.activitiesChange$.unsubscribe();
  }

  private fetchActivities(): void {
    this.isLoading = true;
    this.activityService.fetchActivities()
      .subscribe(
        data => {
          this.activityService.setActivities(data);
          this.isLoading = false;
        },
        error => {
          this.isLoading = false;
        }
      );
  }

}
