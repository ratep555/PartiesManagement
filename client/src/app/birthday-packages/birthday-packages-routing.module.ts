import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BirthdayPackagesComponent } from './birthday-packages.component';
import { BirthdayPackageInfoComponent } from './birthday-package-info/birthday-package-info.component';
import { AddBirthdayClientComponent } from './add-birthday-client/add-birthday-client.component';
import { BirthdaysListComponent } from './birthdays-list/birthdays-list.component';
import { EditBirthdayAdminComponent } from './edit-birthday-admin/edit-birthday-admin.component';
import { BirtdayPackagesAdminComponent } from './birtday-packages-admin/birtday-packages-admin.component';
import { AddBirthdayPackageAdminComponent } from './add-birthday-package-admin/add-birthday-package-admin.component';
import { EditBirthdayPackageAdminComponent } from './edit-birthday-package-admin/edit-birthday-package-admin.component';
import { EditBirthdayPackageAdmin1Component } from './edit-birthday-package-admin1/edit-birthday-package-admin1.component';
import { LocationInfoComponent } from './location-info/location-info.component';
import { LocationsComponent } from './locations/locations.component';
import { SendMessageComponent } from './send-message/send-message.component';


const routes: Routes = [
  {path: '', component: BirthdayPackagesComponent},
  {path: 'birthdaylist', component: BirthdaysListComponent},
  {path: 'birthdaypackagesadmin', component: BirtdayPackagesAdminComponent},
  {path: 'addbirthdaypackageadmin', component: AddBirthdayPackageAdminComponent},
  {path: 'editbirthdaypackageadmin/:id', component: EditBirthdayPackageAdminComponent},
  {path: 'editbirthdaypackageadmin1/:id', component: EditBirthdayPackageAdmin1Component},
  {path: 'birthdaypackageinfo/:id', component: BirthdayPackageInfoComponent},
  {path: 'locationinfo/:id', component: LocationInfoComponent},
  {path: 'locations', component: LocationsComponent},
  {path: 'editbirthday/:id', component: EditBirthdayAdminComponent},
  {path: 'addbirthday', component: AddBirthdayClientComponent},
  {path: 'sendmessage', component: SendMessageComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class BirthdayPackagesRoutingModule { }
