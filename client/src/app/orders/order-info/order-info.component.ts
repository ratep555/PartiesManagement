import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/shared/models/order';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-info',
  templateUrl: './order-info.component.html',
  styleUrls: ['./order-info.component.css']
})
export class OrderInfoComponent implements OnInit {
  order: Order;

  constructor(private ordersService: OrdersService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.getOrderInfoForCustomer();
  }

  getOrderInfoForCustomer() {
    this.ordersService.getOrderInfoForCustomer(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe((order: Order) => {
      this.order = order;
    }, error => {
      console.log(error);
    });
  }

}
