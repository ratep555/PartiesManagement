import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LocationsComponent } from './locations.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { LocationsRoutingModule } from './locations-routing.module';
import { AddLocationComponent } from './add-location/add-location.component';
import { EditLocationComponent } from './edit-location/edit-location.component';



@NgModule({
  declarations: [
    LocationsComponent,
    AddLocationComponent,
    EditLocationComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    LocationsRoutingModule
  ]
})
export class LocationsModule { }
