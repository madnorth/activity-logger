import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, throwError, Subject } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

import { Category } from './category.model';
import { Activity } from './activity.model';
import { ReportItem } from 'src/app/report/shared/report-item.model';
import { ReportResponse } from 'src/app/report/shared/report-response.model';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  private activities: Activity[] = [];
  private apiUrl: string;

  activitiesChanged = new Subject<Activity[]>();

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    this.apiUrl = baseUrl + 'api';
  }

  addActivity(activity: Activity): void {
    this.activities.push(activity);
    this.activitiesChanged.next(this.activities.slice());
  }

  createNewActivity(categoryId: number, startDateTime: string, endDateTime: string, comment: string): Observable<Activity> {
    return this.http
      .post<Activity>(
        this.apiUrl + '/activity',
        {
          categoryId,
          startDateTime,
          endDateTime,
          comment
        }
      )
      .pipe(
        catchError(this.handleError)
      );
  }

  fetchActivities(): Observable<Activity[]> {
    return this.http
      .get<Activity[]>(this.apiUrl + '/activity')
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  fetchReportData(date: string): Observable<ReportResponse> {
    const options = {
      params: new HttpParams().set('date', date)
    }
    return this.http
      .get<ReportResponse>(this.apiUrl + '/activity/report', options)
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  getActivities(): Activity[] {
    return this.activities.slice();
  }

  getAvailableCategories(): Observable<Category[]> {
    return this.http
      .get<Category[]>(this.apiUrl + '/category')
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  setActivities(activities: Activity[]): void {
    this.activities = activities;
    this.activitiesChanged.next(this.activities.slice());
  }

  private handleError(error: HttpErrorResponse) {
    console.error(error);

    return throwError("Something went wrong... Please try again later.");
  }
}
