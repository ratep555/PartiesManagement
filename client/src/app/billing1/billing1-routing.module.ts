import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { Billing1Component } from './billing1.component';
import { Billing1CompletedComponent } from './billing1-completed/billing1-completed.component';


const routes: Routes = [
  {path: '', component: Billing1Component},
  {path: 'completed', component: Billing1CompletedComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]

})
export class Billing1RoutingModule { }
