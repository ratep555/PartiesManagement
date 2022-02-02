import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BillingComponent } from './billing.component';
import { SharedModule } from '../shared/shared.module';
import { BillingRoutingModule } from './billing-routing.module';
import { BillingAddressComponent } from './billing-address/billing-address.component';
import { BillingShippingComponent } from './billing-shipping/billing-shipping.component';
import { BillingOrdersummaryComponent } from './billing-ordersummary/billing-ordersummary.component';
import { BillingPaymentComponent } from './billing-payment/billing-payment.component';
import { BillingCompletedComponent } from './billing-completed/billing-completed.component';



@NgModule({
  declarations: [
    BillingComponent,
    BillingAddressComponent,
    BillingShippingComponent,
    BillingOrdersummaryComponent,
    BillingPaymentComponent,
    BillingCompletedComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    BillingRoutingModule
  ]
})
export class BillingModule { }
