import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ItemsService } from 'src/app/items/items.service';
import { BirthdayPackage, BirthdayPackageCreateEditClass } from 'src/app/shared/models/birthdays/birthdaypackage';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-edit-birthday-package-admin1',
  templateUrl: './edit-birthday-package-admin1.component.html',
  styleUrls: ['./edit-birthday-package-admin1.component.css']
})
export class EditBirthdayPackageAdmin1Component implements OnInit {
  birthdayPackageForms: FormArray = this.fb.array([]);

  model: BirthdayPackage;
  birthdayPackageForm: FormGroup;
  nonSelectedDiscounts: MultipleSelectorModel[] = [];
  selectedDiscounts: MultipleSelectorModel[] = [];
  nonSelectedServices: MultipleSelectorModel[] = [];
  selectedServices: MultipleSelectorModel[] = [];
  id: number;

  constructor(public birthdayPackagesService: BirthdayPackagesService,
              public itemsService: ItemsService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.activatedRoute.params.subscribe(params => {
      this.birthdayPackagesService.putGetBirthdayPackage(params.id).subscribe(putGet => {
        this.model = putGet.BirthdayPackage;
        this.selectedDiscounts = putGet.selectedDiscounts.map(discount => {
          return {key: discount.id, value: discount.name} as MultipleSelectorModel;
        });
        this.nonSelectedDiscounts = putGet.nonSelectedDiscounts.map(discount => {
          return {key: discount.id, value: discount.name} as MultipleSelectorModel;
        });
        this.selectedServices = putGet.selectedServices.map(service => {
          return {key: service.id, value: service.name} as MultipleSelectorModel;
        });
        this.nonSelectedServices = putGet.nonSelectedServices.map(service => {
          return {key: service.id, value: service.name} as MultipleSelectorModel;
        });
      });
    });

    this.birthdayPackagesService.getBirthdayPackage1(this.id).subscribe(
      (birthdayPackage: BirthdayPackageCreateEditClass) => {
      this.birthdayPackageForms.push(this.fb.group({
        id: [this.id],
        packageName: [birthdayPackage.packageName, Validators.required],
        price: [birthdayPackage.price, Validators.required],
        additionalBillingPerParticipant: [birthdayPackage.additionalBillingPerParticipant, Validators.required],
        numberOfParticipants: [birthdayPackage.numberOfParticipants, Validators.required],
        duration: [birthdayPackage.duration, Validators.required],
        description: [birthdayPackage.description, [Validators.required,
          Validators.minLength(10), Validators.maxLength(2000)]],
        discountsIds: [birthdayPackage.discountsIds],
        servicesIds: [birthdayPackage.servicesIds],
        picture: birthdayPackage.picture
      }));
    });

  }

    recordSubmit(fg: FormGroup) {
      const discountsIds = this.selectedDiscounts.map(value => value.key);
      const servicesIds = this.selectedServices.map(value => value.key);

      fg.get('discountsIds').setValue(discountsIds);
      fg.get('servicesIds').setValue(servicesIds);

      this.birthdayPackagesService.updateBirthdayPackage1(fg.value).subscribe(
          (res: any) => {
            this.router.navigateByUrl('birthdaypackages/birthdaypackagesadmin');
          }, error => {
              console.log(error);
            });
          }

    onImageSelected(image){
      this.birthdayPackageForm.get('picture').setValue(image);
      }

}
