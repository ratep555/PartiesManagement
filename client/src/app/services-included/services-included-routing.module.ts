import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ServicesIncludedComponent } from './services-included.component';
import { EditServiceComponent } from './edit-service/edit-service.component';
import { AddServiceComponent } from './add-service/add-service.component';

const routes: Routes = [
  {path: '', component: ServicesIncludedComponent},
  {path: 'addservice', component: AddServiceComponent},
  {path: 'editservice/:id', component: EditServiceComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ServicesIncludedRoutingModule { }
