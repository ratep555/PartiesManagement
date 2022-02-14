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

  constructor(public discountsService: DiscountsService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.discountsService.getAllItems().subscribe(response => {
      this.nonSelectedItems = response.map(item => {
        return  {key: item.id, value: item.name} as MultipleSelectorModel;
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
    });
  }

  onSubmit() {
    const itemsIds = this.selectedItems.map(value => value.key);
    this.discountForm.get('itemsIds').setValue(itemsIds);

    this.discountsService.createDiscount(this.discountForm.value).subscribe(() => {
      this.router.navigateByUrl('discounts');
    },
    error => {
      console.log(error);
    });
  }

}
