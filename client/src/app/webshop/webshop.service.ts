import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Category } from '../shared/models/category';
import { Item } from '../shared/models/item';
import { Manufacturer1 } from '../shared/models/manufacturer1';
import { MyParams } from '../shared/models/myparams';
import { IPaginationForItems, PaginationForItems } from '../shared/models/pagination';
import { Tag } from '../shared/models/tag';

@Injectable({
  providedIn: 'root'
})
export class WebshopService {
  baseUrl = environment.apiUrl;
  products: Item[] = [];
  pagination = new PaginationForItems();
  myParams = new MyParams();
  itemCache = new Map();
  tags: Tag[] = [];
  categories: Category[] = [];
  manufacturers: Manufacturer1[] = [];

  constructor(private http: HttpClient) { }

  getItems() {
   /*  if (useCache === false) {

      this.itemCache = new Map();
    }
    if (this.itemCache.size > 0 && useCache === true) {
      if (this.itemCache.has(Object.values(this.myParams).join('-'))) {
        this.pagination.data = this.itemCache.get(Object.values(this.myParams).join('-'));
        return of(this.pagination);
      }
    } */
    let params = new HttpParams();
    if (this.myParams.manufacturerId !== 0) {
      params = params.append('manufacturerId', this.myParams.manufacturerId.toString());
    }
    if (this.myParams.tagId !== 0) {
      params = params.append('tagId', this.myParams.tagId.toString());
    }
    if (this.myParams.categoryId !== 0) {
      params = params.append('categoryId', this.myParams.categoryId.toString());
    }
    if (this.myParams.query) {
      params = params.append('query', this.myParams.query);
    }
    params = params.append('page', this.myParams.page.toString());
    params = params.append('pageCount', this.myParams.pageCount.toString());
    return this.http.get<IPaginationForItems>(this.baseUrl + 'items', { observe: 'response', params })
      .pipe(
        map(response => {
       //   this.itemCache.set(Object.values(this.myParams).join('-'), response.body.data);
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }

  getItem(id: number) {
    /* let item: Item;
    this.itemCache.forEach((items: Item[]) => {
      console.log(item);
      item = items.find(p => p.id === id);
    });
    if (item) {
      return of(item);
    } */
    return this.http.get<Item>(this.baseUrl + 'items/' + id);
  }

  setMyParams(params: MyParams) {
    this.myParams = params;
  }

  getMyParams() {
    return this.myParams;
  }

  public rate(itemId: number, rating: number){
    return this.http.post(this.baseUrl + 'items/ratings', {itemId, rating});
  }

  decreaseStockQuantity(id: number) {
    return this.http.put(this.baseUrl + 'items/decrease/' + id, {});
  }

  decreaseStockQuantity1(id: number, quantity: number) {
    return this.http.put(this.baseUrl + 'items/decrease1/' + id + '/' + quantity, {});
  }

  increaseStockQuantity1(id: number, quantity: number) {
    return this.http.put(this.baseUrl + 'items/increase1/' + id + '/' + quantity, {});
  }

  increaseStockQuantity(id: number) {
    return this.http.put(this.baseUrl + 'items/increase/' + id, {});
  }

  getAllItemManufacturers() {
    return this.http.get<Manufacturer1[]>(this.baseUrl + 'items/discounts/attributedtoitems');
  }

  getAllItemTags() {
    return this.http.get<Tag[]>(this.baseUrl + 'items/discounts/tagsattributedtoitems');
  }

  getAllItemCategories() {
    return this.http.get<Category[]>(this.baseUrl + 'items/discounts/categoriesattributedtoitems');
  }

  addLike(id: number) {
    return this.http.post(this.baseUrl + 'items/addlike/' + id, {});
  }

}













