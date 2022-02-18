import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Order } from 'src/app/shared/models/order';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-edit-order-new',
  templateUrl: './edit-order-new.component.html',
  styleUrls: ['./edit-order-new.component.css']
})
export class EditOrderNewComponent implements OnInit {
  orderForm: FormGroup;
  model: Order;
  orderstatusesList = [];
  id: number;

  constructor(public ordersService: OrdersService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.ordersService.getOrderStatuses()
    .subscribe(res => this.orderstatusesList = res as []);

    this.orderForm = this.fb.group({
      id: [this.id],
      orderStatus1Id: [null],
     });

    this.ordersService.getOrderInfoForCustomer(this.id)
    .pipe(first())
    .subscribe(x => this.orderForm.patchValue(x));
  }

  onSubmit() {
    if (this.orderForm.invalid) {
        return;
    }
    this.updateOrder();
  }

  private updateOrder() {
    this.ordersService.updateOrder(this.id, this.orderForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('orders');
          }, error => {
            console.log(error);
          });
        }

}









