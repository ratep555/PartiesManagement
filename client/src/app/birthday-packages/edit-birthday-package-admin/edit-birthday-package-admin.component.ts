import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { ItemsService } from 'src/app/items/items.service';
import { BirthdayPackage } from 'src/app/shared/models/birthdays/birthdaypackage';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-edit-birthday-package-admin',
  templateUrl: './edit-birthday-package-admin.component.html',
  styleUrls: ['./edit-birthday-package-admin.component.css']
})
export class EditBirthdayPackageAdminComponent implements OnInit {
  birthdayPackageForm: FormGroup;
  model: BirthdayPackage;
  nonSelectedDiscounts: MultipleSelectorModel[] = [];
  selectedDiscounts: MultipleSelectorModel[] = [];
  nonSelectedServices: MultipleSelectorModel[] = [];
  selectedServices: MultipleSelectorModel[] = [];
  id: number;
  birthdayPackage: BirthdayPackage;


  constructor(public birthdayPackagesService: BirthdayPackagesService,
              public itemsService: ItemsService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.loadBirthdayPackage();

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

    this.birthdayPackageForm = this.fb.group({
      id: [this.id],
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

    this.birthdayPackagesService.getBirthdayPackage(this.id)
    .pipe(first())
    .subscribe(x => this.birthdayPackageForm.patchValue(x));
  }

  loadBirthdayPackage() {
    return this.birthdayPackagesService.getBirthdayPackage(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.birthdayPackage = response;
    }, error => {
    console.log(error);
    });
    }


  onSubmit() {
    const discountsIds = this.selectedDiscounts.map(value => value.key);
    this.birthdayPackageForm.get('discountsIds').setValue(discountsIds);

    const servicesIds = this.selectedServices.map(value => value.key);
    this.birthdayPackageForm.get('servicesIds').setValue(servicesIds);

    this.birthdayPackagesService.updateBirthdayPackage(this.id, this.birthdayPackageForm.value).subscribe(() => {
    this.router.navigateByUrl('birthdaypackages/birthdaypackagesadmin');
        }, error => {
          console.log(error);
        });
      }

      onImageSelected(image){
        this.birthdayPackageForm.get('picture').setValue(image);
      }

}
