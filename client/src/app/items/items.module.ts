import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItemsComponent } from './items.component';
import { AddItemComponent } from './add-item/add-item.component';
import { EditItemComponent } from './edit-item/edit-item.component';
import { InfoItemComponent } from './info-item/info-item.component';
import { SharedModule } from '../shared/shared.module';
import { ItemsRoutingModule } from './items-routing.module';
import { AddItemFelipeComponent } from './add-item-felipe/add-item-felipe.component';
import { EditItemFelipeComponent } from './edit-item-felipe/edit-item-felipe.component';
import { FormItemFelipeComponent } from './form-item-felipe/form-item-felipe.component';
import { InfoItemTabsComponent } from './info-item-tabs/info-item-tabs.component';



@NgModule({
  declarations: [
    ItemsComponent,
    AddItemComponent,
    EditItemComponent,
    InfoItemComponent,
    AddItemFelipeComponent,
    EditItemFelipeComponent,
    FormItemFelipeComponent,
    InfoItemTabsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ItemsRoutingModule
  ]
})
export class ItemsModule { }
