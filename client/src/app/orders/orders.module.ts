import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersComponent } from './orders.component';
import { OrderInfoComponent } from './order-info/order-info.component';
import { SharedModule } from '../shared/shared.module';
import { OrdersRoutingModule } from './orders-routing.module';
import { EditOrderComponent } from './edit-order/edit-order.component';
import { EditOrderNewComponent } from './edit-order-new/edit-order-new.component';



@NgModule({
  declarations: [
    OrdersComponent,
    OrderInfoComponent,
    EditOrderComponent,
    EditOrderNewComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    OrdersRoutingModule
  ]
})
export class OrdersModule { }
