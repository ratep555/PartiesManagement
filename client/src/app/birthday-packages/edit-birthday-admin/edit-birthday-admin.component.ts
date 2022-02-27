import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { OrdersService } from 'src/app/orders/orders.service';
import { BirthdayEdit } from 'src/app/shared/models/birthdays/birthday';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-edit-birthday-admin',
  templateUrl: './edit-birthday-admin.component.html',
  styleUrls: ['./edit-birthday-admin.component.css']
})
export class EditBirthdayAdminComponent implements OnInit {
  birthdayForms: FormArray = this.fb.array([]);
  id: number;
  locationList = [];
  birthdayPackageList = [];
  orderstatusesList = [];


  constructor(private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private birthdayPackagesService: BirthdayPackagesService,
              private ordersService: OrdersService) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.birthdayPackagesService.getLocations()
    .subscribe(res => this.locationList = res as []);

    this.birthdayPackagesService.getBPackages()
    .subscribe(res => this.birthdayPackageList = res as []);

    this.ordersService.getOrderStatuses()
    .subscribe(res => this.orderstatusesList = res as []);

    this.birthdayPackagesService.getBirthdayById(this.id).subscribe(
      (birthday: BirthdayEdit) => {
      this.birthdayForms.push(this.fb.group({
        id: [this.id],
        clientName: [birthday.clientName],
        birthdayGirlBoyName: [birthday.birthdayGirlBoyName],
        birthdayNo: [birthday.birthdayNo],
        numberOfGuests: [birthday.numberOfGuests],
        contactEmail: [birthday.contactEmail],
        contactPhone: [birthday.contactPhone],
        birthdayPackageId: [birthday.birthdayPackageId],
        location1Id: [birthday.location1Id],
        orderStatus1Id: [birthday.orderStatus1Id],
        startDateAndTime: [new Date(birthday.startDateAndTime)],
        endDateAndTime: [new Date(birthday.endDateAndTime)]
        }));
      });

  }

    recordSubmit(fg: FormGroup) {
      this.birthdayPackagesService.updateBirthday(fg.value).subscribe(
          (res: any) => {
            this.router.navigateByUrl('birthdaypackages/birthdaylist');
          }, error => {
              console.log(error);
            });
          }

}
