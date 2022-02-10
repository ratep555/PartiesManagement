import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WebshopComponent } from './webshop.component';
import { ItemComponent } from './item/item.component';
import { ItemDetailComponent } from './item-detail/item-detail.component';
import { SharedModule } from '../shared/shared.module';
import { WebshopRoutingModule } from './webshop-routing.module';
import { ItemDetailTabsComponent } from './item-detail-tabs/item-detail-tabs.component';
import { ItemListIdComponent } from './item-list-id/item-list-id.component';



@NgModule({
  declarations: [
    WebshopComponent,
    ItemComponent,
    ItemDetailComponent,
    ItemDetailTabsComponent,
    ItemListIdComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    WebshopRoutingModule
  ],
  exports: [
    ItemListIdComponent
  ]
})
export class WebshopModule { }
