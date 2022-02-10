import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Item } from 'src/app/shared/models/item';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { ItemsService } from '../items.service';

@Component({
  selector: 'app-edit-item',
  templateUrl: './edit-item.component.html',
  styleUrls: ['./edit-item.component.css']
})
export class EditItemComponent implements OnInit {
  model: Item;
  itemForm: FormGroup;
  nonSelectedCategories: MultipleSelectorModel[] = [];
  selectedCategories: MultipleSelectorModel[] = [];
  nonSelectedDiscounts: MultipleSelectorModel[] = [];
  selectedDiscounts: MultipleSelectorModel[] = [];
  nonSelectedManufacturers: MultipleSelectorModel[] = [];
  selectedManufacturers: MultipleSelectorModel[] = [];
  nonSelectedTags: MultipleSelectorModel[] = [];
  selectedTags: MultipleSelectorModel[] = [];
  id: number;

  constructor(private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private itemsService: ItemsService) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.activatedRoute.params.subscribe(params => {
      this.itemsService.putGetItem(params.id).subscribe(putGet => {
        this.model = putGet.item;
        this.selectedCategories = putGet.selectedCategories.map(category => {
          return {key: category.id, value: category.name} as MultipleSelectorModel;
        });
        this.nonSelectedCategories = putGet.nonSelectedCategories.map(category => {
          return {key: category.id, value: category.name} as MultipleSelectorModel;
        });
        this.selectedDiscounts = putGet.selectedDiscounts.map(discount => {
          return {key: discount.id, value: discount.name} as MultipleSelectorModel;
        });
        this.nonSelectedDiscounts = putGet.nonSelectedDiscounts.map(discount => {
          return {key: discount.id, value: discount.name} as MultipleSelectorModel;
        });
        this.selectedManufacturers = putGet.selectedManufacturers.map(manufacturer => {
          return {key: manufacturer.id, value: manufacturer.name} as MultipleSelectorModel;
        });
        this.nonSelectedManufacturers = putGet.nonSelectedManufacturers.map(manufacturer => {
          return {key: manufacturer.id, value: manufacturer.name} as MultipleSelectorModel;
        });
        this.selectedTags = putGet.selectedTags.map(tag => {
          return {key: tag.id, value: tag.name} as MultipleSelectorModel;
        });
        this.nonSelectedTags = putGet.nonSelectedTags.map(tag => {
          return {key: tag.id, value: tag.name} as MultipleSelectorModel;
        });
      });
    });

    this.itemForm = this.fb.group({
      id: [this.id],
      name: [null, [Validators.required]],
      price: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      categoriesIds: [null],
      discountsIds: [null],
      manufacturersIds: [null],
      tagsIds: [null],
      picture: ''
     });

    this.itemsService.getItemById(this.id)
    .pipe(first())
    .subscribe(x => this.itemForm.patchValue(x));
  }

  onSubmit() {
    const categoriesIds = this.selectedCategories.map(value => value.key);
    this.itemForm.get('categoriesIds').setValue(categoriesIds);

    const discountsIds = this.selectedDiscounts.map(value => value.key);
    this.itemForm.get('discountsIds').setValue(discountsIds);

    const manufacturersIds = this.selectedManufacturers.map(value => value.key);
    this.itemForm.get('manufacturersIds').setValue(manufacturersIds);

    const tagsIds = this.selectedTags.map(value => value.key);
    this.itemForm.get('tagsIds').setValue(tagsIds);
    
  this.itemsService.updateItem(this.id, this.itemForm.value)
      .subscribe(() => {
        this.router.navigateByUrl('items');
        }, error => {
          console.log(error);
        });
      }

      onImageSelected(image){
        this.itemForm.get('picture').setValue(image);
      }

}











