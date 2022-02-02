import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket } from 'src/app/shared/models/basket';
import { Order } from 'src/app/shared/models/order';
import { BillingService } from '../billing.service';

declare var Stripe;

@Component({
  selector: 'app-billing-payment',
  templateUrl: './billing-payment.component.html',
  styleUrls: ['./billing-payment.component.css']
})
export class BillingPaymentComponent implements AfterViewInit, OnDestroy {
  @Input() billingForm: FormGroup;
  @ViewChild('cardNumber', {static: true}) cardNumberElement: ElementRef;
  @ViewChild('cardExpiry', {static: true}) cardExpiryElement: ElementRef;
  @ViewChild('cardCvc', {static: true}) cardCvcElement: ElementRef;
  stripe: any;
  cardNumber: any;
  cardExpiry: any;
  cardCvc: any;
  cardErrors: any;
  cardHandler = this.onChange.bind(this);
  loading = false;
  cardNumberValid = false;
  cardExpiryValid = false;
  cardCvcValid = false;

  constructor(private basketService: BasketService, private checkoutService: BillingService,
              private toastr: ToastrService, private router: Router) { }

  ngAfterViewInit(): void {
    this.stripe = Stripe('pk_test_51KMg0kCSPKMwD05L09eU0COTSwOIz43G6pWE8mRNqBpirNb0wlomkExZWw26R5Sa1qz5Xyv0VYqttQUWSQFHt8Rx00weAv9Aep');
    const elements = this.stripe.elements();

    this.cardNumber = elements.create('cardNumber');
    this.cardNumber.mount(this.cardNumberElement.nativeElement);
    this.cardNumber.addEventListener('change', this.cardHandler);

    this.cardExpiry = elements.create('cardExpiry');
    this.cardExpiry.mount(this.cardExpiryElement.nativeElement);
    this.cardExpiry.addEventListener('change', this.cardHandler);

    this.cardCvc = elements.create('cardCvc');
    this.cardCvc.mount(this.cardCvcElement.nativeElement);
    this.cardCvc.addEventListener('change', this.cardHandler);
  }

  ngOnDestroy() {
    this.cardNumber.destroy();
    this.cardExpiry.destroy();
    this.cardCvc.destroy();
  }

  onChange(event) {
    if (event.error) {
      this.cardErrors = event.error.message;
    } else {
      this.cardErrors = null;
    }
    switch (event.elementType) {
      case 'cardNumber':
        this.cardNumberValid = event.complete;
        break;
      case 'cardExpiry':
        this.cardExpiryValid = event.complete;
        break;
      case 'cardCvc':
        this.cardCvcValid = event.complete;
        break;
    }
  }

  submitOrder() {
    const basket = this.basketService.gettingValueOfBasket();
    const orderToCreate = this.getOrderToCreate(basket);
    this.checkoutService.creatingOrder(orderToCreate).subscribe((order: Order) => {
      this.toastr.success('Order created successfully');
      this.stripe.confirmCardPayment(basket.clientSecret, {
        payment_method: {
          card: this.cardNumber,
          billing_details: {
            name: this.billingForm.get('paymentForm').get('nameOnCard').value
          }
        }
      }).then(result => {
        console.log(result);
        if (result.paymentIntent) {
       this.basketService.deletingBasket(basket);
       const navigationExtras: NavigationExtras = {state: order};
       this.router.navigate(['billing/completed'], navigationExtras);
        } else {
          this.toastr.error(result.error.message);
        }
      });
    }, error => {
      this.toastr.error(error.message);
      console.log(error);
    });
  }

  private getOrderToCreate(basket: Basket) {
    return {
      basketId: basket.id,
      shippingOptionId: +this.billingForm.get('shippingForm').get('shippingOption').value,
      shippingAddress: this.billingForm.get('addressForm').value
    };
  }

}
