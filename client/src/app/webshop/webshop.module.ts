import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WebshopComponent } from './webshop.component';
import { ItemComponent } from './item/item.component';
import { ItemDetailComponent } from './item-detail/item-detail.component';
import { SharedModule } from '../shared/shared.module';
import { WebshopRoutingModule } from './webshop-routing.module';



@NgModule({
  declarations: [
    WebshopComponent,
    ItemComponent,
    ItemDetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    WebshopRoutingModule
  ]
})
export class WebshopModule { }
