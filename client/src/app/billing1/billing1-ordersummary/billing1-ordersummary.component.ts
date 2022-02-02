import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { BillingService } from 'src/app/billing/billing.service';
import { Basket } from 'src/app/shared/models/basket';
import { Order } from 'src/app/shared/models/order';
import { PayingOption } from 'src/app/shared/models/payingOption';
import { Billing1Service } from '../billing1.service';

@Component({
  selector: 'app-billing1-ordersummary',
  templateUrl: './billing1-ordersummary.component.html',
  styleUrls: ['./billing1-ordersummary.component.css']
})
export class Billing1OrdersummaryComponent implements OnInit {
  @Input() billingForm: FormGroup;
  @Input() appStepper: CdkStepper;
  basket$: Observable<Basket>;
  stripe: PayingOption;

  constructor(private basketService: BasketService,
              private billingService: Billing1Service,
              private toastr: ToastrService,
              private router: Router) { }

  ngOnInit() {
    this.basket$ = this.basketService.basket$;
    this.billingService.getStripePayingOption()
    .subscribe(res => this.stripe = res);
  }

   createPaymentIntent() {
     if (+this.billingForm.get('payingForm').get('payingOption').value === this.stripe.id) {
      return this.basketService.createPaymentIntent().subscribe((response: any) => {
        this.toastr.success('Payment intent created');
        console.log(this.basketService.gettingValueOfBasket());
        this.appStepper.next();
         }, error => {
          console.log(error);
        });
      } else {
        const basket = this.basketService.gettingValueOfBasket();
        const orderToCreate = this.getOrderToCreate(basket);
        this.billingService.creatingOrder(orderToCreate).subscribe((order: Order) => {
        this.basketService.deletingBasket(basket);
        this.router.navigate(['billing/completed']);
        });
      }
  }

   createPaymentIntent1() {
      return this.basketService.createPaymentIntent().subscribe((response: any) => {
        this.toastr.success('Payment intent created');
        console.log(this.basketService.gettingValueOfBasket());
        this.appStepper.next();
         }, error => {
          console.log(error);
        });
      }


  createOrder() {
    const basket = this.basketService.gettingValueOfBasket();
    const orderToCreate = this.getOrderToCreate(basket);
    this.billingService.creatingOrder(orderToCreate).subscribe((order: Order) => {
    this.basketService.deletingBasket(basket);
    this.router.navigate(['billing/completed']);
    });
  }

  private getOrderToCreate(basket: Basket) {
    return {
      basketId: basket.id,
      shippingOptionId: +this.billingForm.get('shippingForm').get('shippingOption').value,
      paymentOptionId: +this.billingForm.get('payingForm').get('payingOption').value,
      shippingAddress: this.billingForm.get('addressForm').value
    };
  }

}
