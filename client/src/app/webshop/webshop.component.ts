import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Item } from '../shared/models/item';
import { Manufacturer } from '../shared/models/manufacturer';
import { MyParams } from '../shared/models/myparams';
import { WebshopService } from './webshop.service';
import { DiscountsService } from '../admin/discounts/discounts.service';
import { Tag } from '../shared/models/tag';
import { Category } from '../shared/models/category';

@Component({
  selector: 'app-webshop',
  templateUrl: './webshop.component.html',
  styleUrls: ['./webshop.component.css']
})
export class WebshopComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('filter', {static: false}) filterTerm: ElementRef;
  @ViewChild('filter1', {static: false}) filterTerm1: ElementRef;
  items: Item[];
  myParams: MyParams;
  totalCount: number;
  manufacturers: Manufacturer[];
  tags: Tag[];
  categories: Category[];

  constructor(private webshopService: WebshopService,
              private discountsService: DiscountsService) {
    this.myParams = this.webshopService.getMyParams();
   }

  ngOnInit(): void {
    this.getItems();
    this.getManufacturers();
    this.getTags();
    this.getCategories();
  }

  getItems() {
    this.webshopService.getItems().subscribe(response => {
      this.items = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  getManufacturers() {
    this.webshopService.getAllItemManufacturers().subscribe(response => {
    this.manufacturers = response;
    }, error => {
    console.log(error);
    });
    }

  getTags() {
    this.webshopService.getAllItemTags().subscribe(response => {
    this.tags = response;
    }, error => {
    console.log(error);
    });
    }

  getCategories() {
    this.webshopService.getAllItemCategories().subscribe(response => {
    this.categories = response;
    }, error => {
    console.log(error);
    });
    }

  onManufacturerSelected(manufacturerId: number) {
    const params = this.webshopService.getMyParams();
    params.manufacturerId = manufacturerId;
    params.page = 1;
    this.webshopService.setMyParams(params);
    this.getItems();
    }

  onCategorySelected(categoryId: number) {
    const params = this.webshopService.getMyParams();
    params.categoryId = categoryId;
    params.page = 1;
    this.webshopService.setMyParams(params);
    this.getItems();
    }

  onTagSelected(tagId: number) {
    const params = this.webshopService.getMyParams();
    params.tagId = tagId;
    params.page = 1;
    this.webshopService.setMyParams(params);
    this.getItems();
    }

  onPageChanged(event: any) {
    const params = this.webshopService.getMyParams();
    if (params.page !== event) {
      params.page = event;
      this.webshopService.setMyParams(params);
      this.getItems();
    }
  }

  onSearch() {
    const params = this.webshopService.getMyParams();
    params.query = this.searchTerm.nativeElement.value;
    params.page = 1;
    this.webshopService.setMyParams(params);
    this.getItems();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.webshopService.setMyParams(this.myParams);
    this.getItems();
  }

  onReset1() {
    this.filterTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.webshopService.setMyParams(this.myParams);
    this.getItems();
  }

  onReset2() {
    this.filterTerm1.nativeElement.value = '';
    this.myParams = new MyParams();
    this.webshopService.setMyParams(this.myParams);
    this.getItems();
  }

}

















