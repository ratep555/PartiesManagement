import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ItemsComponent } from './items.component';
import { AddItemComponent } from './add-item/add-item.component';
import { EditItemComponent } from './edit-item/edit-item.component';
import { InfoItemComponent } from './info-item/info-item.component';
import { AddItemFelipeComponent } from './add-item-felipe/add-item-felipe.component';
import { EditItemFelipeComponent } from './edit-item-felipe/edit-item-felipe.component';
import { EnumAttemptComponent } from './enum-attempt/enum-attempt.component';

const routes: Routes = [
  {path: '', component: ItemsComponent},
  {path: 'additem', component: AddItemComponent},
  {path: 'edititem/:id', component: EditItemComponent},
  {path: 'additemfelipe', component: AddItemFelipeComponent},
  {path: 'edititemfelipe/:id', component: EditItemFelipeComponent},
  {path: 'infoitem/:id', component: InfoItemComponent},
  {path: 'enum', component: EnumAttemptComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ItemsRoutingModule { }
