import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { OrderStatus } from 'src/app/shared/models/enums';
import { Order } from 'src/app/shared/models/order';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html',
  styleUrls: ['./edit-order.component.css']
})
export class EditOrderComponent implements OnInit {
  orderForm: FormGroup;
  order: Order;
  id: number;
  orderStatuses = OrderStatus;
  enumKeys = [];

  constructor(private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private ordersService: OrdersService,
              private router: Router)
              {this.enumKeys = Object.keys(this.orderStatuses); }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.orderForm = this.fb.group({
      id: [this.id],
      orderStatus: [''],
     });

    this.ordersService.getOrderInfoForCustomer(this.id)
    .pipe(first())
    .subscribe(x => this.orderForm.patchValue(x));
  }

  /* onSubmit() {
    if (this.orderForm.invalid) {
        return;
    }
    this.updateOrder();
  }

  private updateOrder() {
  this.ordersService.updateHospital(this.id, this.hospitalForm.value)
      .pipe(first())
      .subscribe(() => {
        this.router.navigateByUrl('admin/hospitalslistadmin');
        }, error => {
          console.log(error);
        });
      }
  }
 */
}
