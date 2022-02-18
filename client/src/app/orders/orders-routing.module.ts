import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { OrdersComponent } from './orders.component';
import { OrderInfoComponent } from './order-info/order-info.component';
import { EditOrderComponent } from './edit-order/edit-order.component';
import { EditOrderNewComponent } from './edit-order-new/edit-order-new.component';

const routes: Routes = [
  {path: '', component: OrdersComponent},
  {path: ':id', component: OrderInfoComponent},
  {path: 'editorder/:id', component: EditOrderComponent},
  {path: 'editorder1/:id', component: EditOrderNewComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
