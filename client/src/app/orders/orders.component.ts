import { Component, OnInit } from '@angular/core';
import { Order } from '../shared/models/order';
import { OrdersService } from './orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
orders: Order[];

  constructor(private ordersService: OrdersService) { }

  ngOnInit(): void {
    this.getOrdersForCustomer();
  }

  getOrdersForCustomer() {
    this.ordersService.getOrdersForCustomer().subscribe((orders: Order[]) => {
      this.orders = orders;
    }, error => {
      console.log(error);
    });
  }

}
