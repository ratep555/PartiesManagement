import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LocationsComponent } from './locations.component';
import { AddLocationComponent } from './add-location/add-location.component';
import { EditLocationComponent } from './edit-location/edit-location.component';

const routes: Routes = [
  {path: '', component: LocationsComponent},
  {path: 'addlocation', component: AddLocationComponent},
  {path: 'editlocation/:id', component: EditLocationComponent},

];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class LocationsRoutingModule { }
