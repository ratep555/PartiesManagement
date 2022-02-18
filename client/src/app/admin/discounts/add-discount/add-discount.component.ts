import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Discount } from 'src/app/shared/models/discount';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { DiscountsService } from '../discounts.service';

@Component({
  selector: 'app-add-discount',
  templateUrl: './add-discount.component.html',
  styleUrls: ['./add-discount.component.css']
})
export class AddDiscountComponent implements OnInit {
  discountForm: FormGroup;
  model: Discount;
  nonSelectedItems: MultipleSelectorModel[] = [];
  selectedItems: MultipleSelectorModel[] = [];
  nonSelectedCategories: MultipleSelectorModel[] = [];
  selectedCategories: MultipleSelectorModel[] = [];
  nonSelectedManufacturers: MultipleSelectorModel[] = [];
  selectedManufacturers: MultipleSelectorModel[] = [];

  constructor(public discountsService: DiscountsService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.discountsService.getAllItems().subscribe(response => {
      this.nonSelectedItems = response.map(item => {
        return  {key: item.id, value: item.name} as MultipleSelectorModel;
      });
    });

    this.discountsService.getAllCategories().subscribe(response => {
      this.nonSelectedCategories = response.map(category => {
        return  {key: category.id, value: category.name} as MultipleSelectorModel;
      });
    });

    this.discountsService.getAllManufacturers().subscribe(response => {
      this.nonSelectedManufacturers = response.map(manufacturer => {
        return  {key: manufacturer.id, value: manufacturer.name} as MultipleSelectorModel;
      });
    });

    this.createDiscountForm();
  }

  createDiscountForm() {
    this.discountForm = this.fb.group({
      name: [null, [Validators.required]],
      discountPercentage: ['', [Validators.required]],
      minimumOrderValue: ['', [Validators.required]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      itemsIds: [null],
      categoriesIds: [null],
      manufacturersIds: [null]
    });
  }

  onSubmit() {
    const itemsIds = this.selectedItems.map(value => value.key);
    this.discountForm.get('itemsIds').setValue(itemsIds);

    const categoriesIds = this.selectedCategories.map(value => value.key);
    this.discountForm.get('categoriesIds').setValue(categoriesIds);

    const manufacturersIds = this.selectedManufacturers.map(value => value.key);
    this.discountForm.get('manufacturersIds').setValue(manufacturersIds);

    this.discountsService.createDiscount(this.discountForm.value).subscribe(() => {
      this.router.navigateByUrl('discounts');
    },
    error => {
      console.log(error);
    });
  }

}
