import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Item } from 'src/app/shared/models/item';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { ItemsService } from '../items.service';

@Component({
  selector: 'app-add-item',
  templateUrl: './add-item.component.html',
  styleUrls: ['./add-item.component.css']
})
export class AddItemComponent implements OnInit {
  itemForm: FormGroup;
  model: Item;
  nonSelectedCategories: MultipleSelectorModel[] = [];
  selectedCategories: MultipleSelectorModel[] = [];
  nonSelectedDiscounts: MultipleSelectorModel[] = [];
  selectedDiscounts: MultipleSelectorModel[] = [];
  nonSelectedManufacturers: MultipleSelectorModel[] = [];
  selectedManufacturers: MultipleSelectorModel[] = [];
  nonSelectedTags: MultipleSelectorModel[] = [];
  selectedTags: MultipleSelectorModel[] = [];

  constructor(public itemsService: ItemsService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.itemsService.getAllCategories().subscribe(response => {
      this.nonSelectedCategories = response.map(category => {
        return  {key: category.id, value: category.name} as MultipleSelectorModel;
      });
    });

    this.itemsService.getAllDiscounts().subscribe(response => {
      this.nonSelectedDiscounts = response.map(discount => {
        return  {key: discount.id, value: discount.name} as MultipleSelectorModel;
      });
    });

    this.itemsService.getAllManufacturers().subscribe(response => {
      this.nonSelectedManufacturers = response.map(manufacturer => {
        return  {key: manufacturer.id, value: manufacturer.name} as MultipleSelectorModel;
      });
    });

    this.itemsService.getAllTags().subscribe(response => {
      this.nonSelectedTags = response.map(tag => {
        return  {key: tag.id, value: tag.name} as MultipleSelectorModel;
      });
    });

    this.createItemForm();
  }

  createItemForm() {
    this.itemForm = this.fb.group({
      name: [null, [Validators.required]],
      price: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      categoriesIds: [null],
      manufacturersIds: [null],
      tagsIds: [null],
      discountsIds: [null],
      picture: ''
    });
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

    this.itemsService.createItem(this.itemForm.value).subscribe(() => {
      this.router.navigateByUrl('items');
    },
    error => {
      console.log(error);
    });
  }

  onImageSelected(image){
    this.itemForm.get('picture').setValue(image);
  }

}
