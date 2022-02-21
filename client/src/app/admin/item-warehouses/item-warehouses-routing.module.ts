import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ItemWarehousesComponent } from './item-warehouses.component';
import { AddItemWarehouseComponent } from './add-item-warehouse/add-item-warehouse.component';
import { EditItemWarehouseComponent } from './edit-item-warehouse/edit-item-warehouse.component';

const routes: Routes = [
  {path: '', component: ItemWarehousesComponent},
  {path: 'additemwarehouse', component: AddItemWarehouseComponent},
  {path: 'edititemwarehouse/:id/:warehouseid', component: EditItemWarehouseComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ItemWarehousesRoutingModule { }
