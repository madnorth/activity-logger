import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ActivitiesComponent } from './activities/activities.component';


const routes: Routes = [
  { path: '', component: ActivitiesComponent },
  { path: 'fetch-data', component: FetchDataComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
