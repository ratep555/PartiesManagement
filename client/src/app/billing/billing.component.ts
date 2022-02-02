import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { BasketService } from '../basket/basket.service';

@Component({
  selector: 'app-billing',
  templateUrl: './billing.component.html',
  styleUrls: ['./billing.component.css']
})
export class BillingComponent implements OnInit {
  billingForm: FormGroup;

  constructor(private accountService: AccountService,
              private basketService: BasketService,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createBillingForm();
    this.patchCustomerAddress();
    this.patchCustomerShippingOption();
  }

  createBillingForm() {
    this.billingForm = this.fb.group({
      addressForm: this.fb.group({
        firstName: [null, Validators.required],
        lastName: [null, Validators.required],
        street: [null, Validators.required],
        city: [null, Validators.required],
        countryId: [0, [Validators.min(1)]]
      }),
      shippingForm: this.fb.group({
        shippingOption: [null, Validators.required]
      }),
      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required]
      })
    });
  }

  patchCustomerAddress() {
    this.accountService.getCustomerAddress().subscribe(address => {
      if (address) {
        this.billingForm.get('addressForm').patchValue(address);
      }
    }, error => {
      console.log(error);
    });
  }

  patchCustomerShippingOption() {
    const basket = this.basketService.gettingValueOfBasket();
    if (basket.shippingOptionId !== null) {
      this.billingForm.get('shippingForm').get('shippingOption').patchValue(basket.shippingOptionId.toString());
    }
  }

}
