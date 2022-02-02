import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { PayingOption } from 'src/app/shared/models/payingOption';
import { Billing1Service } from '../billing1.service';

@Component({
  selector: 'app-billing1-paymentoption',
  templateUrl: './billing1-paymentoption.component.html',
  styleUrls: ['./billing1-paymentoption.component.css']
})
export class Billing1PaymentoptionComponent implements OnInit {
  @Input() billingForm: FormGroup;
  payingOptions: PayingOption[];

  constructor(private billingService: Billing1Service,
              private basketService: BasketService) { }

  ngOnInit(): void {
    this.getPayingOptions();
  }

  getPayingOptions() {
    this.billingService.getPayingOptions().subscribe((payingOptions: PayingOption[]) => {
      this.payingOptions = payingOptions;
    }, error => {
      console.log(error);
    });
  }

  persistingPayingOption(payingOption: PayingOption) {
    this.basketService.persistingPayingOption(payingOption);
  }

}
