import { Component, Input, OnInit } from '@angular/core';
import { CdkStepper } from '@angular/cdk/stepper';
import { Observable } from 'rxjs';
import { Basket } from 'src/app/shared/models/basket';
import { BasketService } from 'src/app/basket/basket.service';
import { ToastrService } from 'ngx-toastr';
import { FormGroup } from '@angular/forms';


@Component({
  selector: 'app-billing-ordersummary',
  templateUrl: './billing-ordersummary.component.html',
  styleUrls: ['./billing-ordersummary.component.css']
})
export class BillingOrdersummaryComponent implements OnInit {
  @Input() billingForm: FormGroup;
  @Input() appStepper: CdkStepper;
  basket$: Observable<Basket>;

  constructor(private basketService: BasketService,
              private toastr: ToastrService) { }

  ngOnInit() {
    this.basket$ = this.basketService.basket$;
  }

   createPaymentIntent() {

    return this.basketService.createPaymentIntent().subscribe((response: any) => {
    this.toastr.success('Payment intent created');
    console.log(this.basketService.gettingValueOfBasket());
    this.appStepper.next();
     }, error => {
      console.log(error);
    });
  }

}
