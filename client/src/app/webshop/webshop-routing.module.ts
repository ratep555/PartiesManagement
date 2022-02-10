import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { WebshopComponent } from './webshop.component';
import { ItemDetailComponent } from './item-detail/item-detail.component';
import { ItemListIdComponent } from './item-list-id/item-list-id.component';

const routes: Routes = [
  {path: '', component: WebshopComponent},
  {path: ':id', component: ItemDetailComponent},
  {path: 'itemlistid/:id', component: ItemListIdComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class WebshopRoutingModule { }
