import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Billing1Component } from './billing1.component';
import { Billing1AddressComponent } from './billing1-address/billing1-address.component';
import { Billing1ShippingoptionComponent } from './billing1-shippingoption/billing1-shippingoption.component';
import { Billing1PaymentoptionComponent } from './billing1-paymentoption/billing1-paymentoption.component';
import { Billing1OrdersummaryComponent } from './billing1-ordersummary/billing1-ordersummary.component';
import { Billing1CompletedComponent } from './billing1-completed/billing1-completed.component';
import { SharedModule } from '../shared/shared.module';
import { Billing1RoutingModule } from './billing1-routing.module';
import { Billing1StripeComponent } from './billing1-stripe/billing1-stripe.component';



@NgModule({
  declarations: [
    Billing1Component,
    Billing1AddressComponent,
    Billing1ShippingoptionComponent,
    Billing1PaymentoptionComponent,
    Billing1OrdersummaryComponent,
    Billing1CompletedComponent,
    Billing1StripeComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    Billing1RoutingModule
  ]
})
export class Billing1Module { }
