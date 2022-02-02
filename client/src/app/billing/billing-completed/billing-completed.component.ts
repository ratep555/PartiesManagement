import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Order } from 'src/app/shared/models/order';

@Component({
  selector: 'app-billing-completed',
  templateUrl: './billing-completed.component.html',
  styleUrls: ['./billing-completed.component.css']
})
export class BillingCompletedComponent implements OnInit {
  order: Order;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation && navigation.extras && navigation.extras.state;
    if (state) {
      this.order = state as Order;
    }
   }

  ngOnInit(): void {
  }

}
