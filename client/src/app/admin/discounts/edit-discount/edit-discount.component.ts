import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Discount, DiscountEditClass } from 'src/app/shared/models/discount';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { DiscountsService } from '../discounts.service';

@Component({
  selector: 'app-edit-discount',
  templateUrl: './edit-discount.component.html',
  styleUrls: ['./edit-discount.component.css']
})
export class EditDiscountComponent implements OnInit {
  discountForms: FormArray = this.fb.array([]);

  model: Discount;
  discountForm: FormGroup;
  nonSelectedItems: MultipleSelectorModel[] = [];
  selectedItems: MultipleSelectorModel[] = [];
  id: number;

  constructor(private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private discountsService: DiscountsService) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.activatedRoute.params.subscribe(params => {
      this.discountsService.putGetDiscount(params.id).subscribe(putGet => {
        this.model = putGet.discount;

        this.selectedItems = putGet.selectedItems.map(item => {
          return {key: item.id, value: item.name} as MultipleSelectorModel;
        });
        this.nonSelectedItems = putGet.nonSelectedItems.map(item => {
          return {key: item.id, value: item.name} as MultipleSelectorModel;
        });

      });
    });

    /* this.discountForm = this.fb.group({
      id: [this.id],
      name: [null, [Validators.required]],
      discountPercentage: ['', [Validators.required]],
      minimumOrderValue: ['', [Validators.required]],
      startDate: [new Date(), Validators.required],
      endDate: [new Date(), Validators.required],
      itemsIds: [null],
     });

    this.discountsService.getDiscountById(this.id)
    .pipe(first())
    .subscribe(x => this.discountForm.patchValue(x));
 */
    this.discountsService.getDiscountById(this.id).subscribe(
      (discount: DiscountEditClass) => {
      this.discountForms.push(this.fb.group({
        id: [this.id],
        name: [discount.name, Validators.required],
        discountPercentage: [discount.discountPercentage, Validators.required],
        minimumOrderValue: [discount.minimumOrderValue, Validators.required],
        itemsIds: [discount.items, Validators.required],
        startDate: [new Date(discount.startDate), Validators.required],
        endDate: [new Date(discount.endDate), Validators.required],
        }));
      });

  }

  onSubmit() {
    const itemsIds = this.selectedItems.map(value => value.key);
    this.discountForm.get('itemsIds').setValue(itemsIds);

    this.discountsService.updateDiscount(this.id, this.discountForm.value).subscribe(() => {
    this.router.navigateByUrl('discounts');
        }, error => {
          console.log(error);
        });
      }

    recordSubmit(fg: FormGroup) {
      const itemsIds = this.selectedItems.map(value => value.key);

      fg.get('itemsIds').setValue(itemsIds);

      this.discountsService.updateDiscount1(fg.value).subscribe(
          (res: any) => {
            this.router.navigateByUrl('discounts');
          }, error => {
              console.log(error);
            });
          }

}
