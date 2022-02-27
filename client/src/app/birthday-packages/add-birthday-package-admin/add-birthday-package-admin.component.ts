import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ItemsService } from 'src/app/items/items.service';
import { BirthdayPackage } from 'src/app/shared/models/birthdays/birthdaypackage';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-add-birthday-package-admin',
  templateUrl: './add-birthday-package-admin.component.html',
  styleUrls: ['./add-birthday-package-admin.component.css']
})
export class AddBirthdayPackageAdminComponent implements OnInit {
  birthdayPackageForm: FormGroup;
  model: BirthdayPackage;
  nonSelectedDiscounts: MultipleSelectorModel[] = [];
  selectedDiscounts: MultipleSelectorModel[] = [];
  nonSelectedServices: MultipleSelectorModel[] = [];
  selectedServices: MultipleSelectorModel[] = [];

  constructor(public birthdayPackagesService: BirthdayPackagesService,
              public itemsService: ItemsService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.itemsService.getAllDiscounts().subscribe(response => {
      this.nonSelectedDiscounts = response.map(discount => {
        return  {key: discount.id, value: discount.name} as MultipleSelectorModel;
      });
    });

    this.birthdayPackagesService.getAllServices().subscribe(response => {
      this.nonSelectedServices = response.map(service => {
        return  {key: service.id, value: service.name} as MultipleSelectorModel;
      });
    });

    this.createBirthdayPackageForm();
  }

  createBirthdayPackageForm() {
    this.birthdayPackageForm = this.fb.group({
      packageName: [null, [Validators.required]],
      price: ['', [Validators.required]],
      additionalBillingPerParticipant: ['', [Validators.required]],
      numberOfParticipants: ['', [Validators.required]],
      duration: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      discountsIds: [null],
      servicesIds: [null],
      picture: ''
    });
  }

  onSubmit() {
    const servicesIds = this.selectedServices.map(value => value.key);
    this.birthdayPackageForm.get('servicesIds').setValue(servicesIds);

    const discountsIds = this.selectedDiscounts.map(value => value.key);
    this.birthdayPackageForm.get('discountsIds').setValue(discountsIds);

    this.birthdayPackagesService.createBirthdayPackage(this.birthdayPackageForm.value).subscribe(() => {
      this.router.navigateByUrl('birthdaypackages/birthdaypackagesadmin');
    },
    error => {
      console.log(error);
    });
  }

  onImageSelected(image){
    this.birthdayPackageForm.get('picture').setValue(image);
  }

}
