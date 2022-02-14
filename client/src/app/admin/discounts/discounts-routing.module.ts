import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { DiscountsComponent } from './discounts.component';
import { AddDiscountComponent } from './add-discount/add-discount.component';
import { EditDiscountComponent } from './edit-discount/edit-discount.component';

const routes: Routes = [
  {path: '', component: DiscountsComponent},
  {path: 'adddiscount', component: AddDiscountComponent},
  {path: 'editdiscount/:id', component: EditDiscountComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class DiscountsRoutingModule { }
