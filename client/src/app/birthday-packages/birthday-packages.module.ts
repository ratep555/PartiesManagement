import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BirthdayPackagesComponent } from './birthday-packages.component';
import { BirthdayPackageComponent } from './birthday-package/birthday-package.component';
import { SharedModule } from '../shared/shared.module';
import { BirthdayPackagesRoutingModule } from './birthday-packages-routing.module';
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
import { ServicesIncludedComponent } from './services-included/services-included.component';
import { ServiceIncludedComponent } from './service-included/service-included.component';
import { ServiceIncludedInfoComponent } from './service-included-info/service-included-info.component';



@NgModule({
  declarations: [
    BirthdayPackagesComponent,
    BirthdayPackageComponent,
    BirthdayPackageInfoComponent,
    AddBirthdayClientComponent,
    BirthdaysListComponent,
    EditBirthdayAdminComponent,
    BirtdayPackagesAdminComponent,
    AddBirthdayPackageAdminComponent,
    EditBirthdayPackageAdminComponent,
    EditBirthdayPackageAdmin1Component,
    LocationInfoComponent,
    LocationsComponent,
    SendMessageComponent,
    ServicesIncludedComponent,
    ServiceIncludedComponent,
    ServiceIncludedInfoComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    BirthdayPackagesRoutingModule
  ]
})
export class BirthdayPackagesModule { }
