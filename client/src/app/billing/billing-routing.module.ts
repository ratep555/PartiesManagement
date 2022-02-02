import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BillingComponent } from './billing.component';
import { BillingCompletedComponent } from './billing-completed/billing-completed.component';


const routes: Routes = [
  {path: '', component: BillingComponent},
  {path: 'completed', component: BillingCompletedComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]

})
export class BillingRoutingModule { }
