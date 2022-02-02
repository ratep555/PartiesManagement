import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItemsComponent } from './items.component';
import { AddItemComponent } from './add-item/add-item.component';
import { EditItemComponent } from './edit-item/edit-item.component';
import { InfoItemComponent } from './info-item/info-item.component';
import { SharedModule } from '../shared/shared.module';
import { ItemsRoutingModule } from './items-routing.module';



@NgModule({
  declarations: [
    ItemsComponent,
    AddItemComponent,
    EditItemComponent,
    InfoItemComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ItemsRoutingModule
  ]
})
export class ItemsModule { }
