import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ActivitiesComponent } from './activities/activities.component';
import { ReportComponent } from './report/report.component';


const routes: Routes = [
  { path: '', component: ActivitiesComponent },
  { path: 'daily-report', component: ReportComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
