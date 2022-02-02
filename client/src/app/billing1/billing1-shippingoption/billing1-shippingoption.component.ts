import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { ShippingOption } from 'src/app/shared/models/shippingOption';
import { Billing1Service } from '../billing1.service';

@Component({
  selector: 'app-billing1-shippingoption',
  templateUrl: './billing1-shippingoption.component.html',
  styleUrls: ['./billing1-shippingoption.component.css']
})
export class Billing1ShippingoptionComponent implements OnInit {
  @Input() billingForm: FormGroup;
  shippingOptions: ShippingOption[];

  constructor(private billingService: Billing1Service,
              private basketService: BasketService) { }

  ngOnInit(): void {
    this.getShippingOptions();
  }

  getShippingOptions() {
    this.billingService.getShippingOptions().subscribe((shippingOptions: ShippingOption[]) => {
      this.shippingOptions = shippingOptions;
    }, error => {
      console.log(error);
    });
  }

  calculateShippingPrice(shippingOption: ShippingOption) {
    this.basketService.calculateShippingPrice(shippingOption);
  }

}
