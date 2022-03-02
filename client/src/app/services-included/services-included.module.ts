import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ServicesIncludedComponent } from './services-included.component';
import { AddServiceComponent } from './add-service/add-service.component';
import { EditServiceComponent } from './edit-service/edit-service.component';
import { SharedModule } from '../shared/shared.module';
import { ServicesIncludedRoutingModule } from './services-included-routing.module';



@NgModule({
  declarations: [
    ServicesIncludedComponent,
    AddServiceComponent,
    EditServiceComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ServicesIncludedRoutingModule
  ]
})
export class ServicesIncludedModule { }
