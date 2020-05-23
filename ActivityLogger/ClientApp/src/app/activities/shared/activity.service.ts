import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { Category } from './category.model';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  baseUrl: string;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') _baseUrl: string
  ) {
    this.baseUrl = _baseUrl;
  }

  getAvailableCategories(): Observable<Category[]> {
    return this.http
      .get<Category[]>(this.baseUrl + 'api/category')
      .pipe(
        retry(3),
        catchError(this.handleError)
      )
  }

  private handleError(error: HttpErrorResponse) {
    console.error(error);

    return throwError("Please try again later.");
  }
}
