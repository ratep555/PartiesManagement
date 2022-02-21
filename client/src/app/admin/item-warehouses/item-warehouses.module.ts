import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItemWarehousesComponent } from './item-warehouses.component';
import { AddItemWarehouseComponent } from './add-item-warehouse/add-item-warehouse.component';
import { EditItemWarehouseComponent } from './edit-item-warehouse/edit-item-warehouse.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ItemWarehousesRoutingModule } from './item-warehouses-routing.module';



@NgModule({
  declarations: [
    ItemWarehousesComponent,
    AddItemWarehouseComponent,
    EditItemWarehouseComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ItemWarehousesRoutingModule
  ]
})
export class ItemWarehousesModule { }
