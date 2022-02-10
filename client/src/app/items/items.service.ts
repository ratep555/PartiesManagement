import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { Category } from '../shared/models/category';
import { Discount } from '../shared/models/discount';
import { Item, ItemCreateEdit, ItemEdit, ItemPutGet } from '../shared/models/item';
import { Manufacturer } from '../shared/models/manufacturer';
import { MyParams, UserParams } from '../shared/models/myparams';
import { IPaginationForItems, PaginationForItems } from '../shared/models/pagination';
import { Tag } from '../shared/models/tag';
import { User } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class ItemsService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;

  constructor(private http: HttpClient,
              private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take (1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
});
}

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getItems(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<PaginationForItems>(this.baseUrl + 'items', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getItemById(id: number) {
    return this.http.get<Item>(this.baseUrl + 'items/' + id);
  }

  createItem(item: ItemCreateEdit) {
    const formData = this.BuildFormData(item);
    return this.http.post(this.baseUrl + 'items', formData);
  }

  updateItem(id: number, item: ItemEdit){
    const formData = this.BuildFormData1(item);
    return this.http.put(this.baseUrl + 'items/' + id, formData);
  }

  putGetItem(id: number): Observable<ItemPutGet>{
    return this.http.get<ItemPutGet>(this.baseUrl + 'items/putget1/' + id);
  }

  getAllCategories() {
    return this.http.get<Category[]>(this.baseUrl + 'items/categories');
  }

  getAllDiscounts() {
    return this.http.get<Discount[]>(this.baseUrl + 'items/discounts');
  }

  getAllManufacturers() {
    return this.http.get<Manufacturer[]>(this.baseUrl + 'items/manufacturers');
  }

  getAllTags() {
    return this.http.get<Tag[]>(this.baseUrl + 'items/tags');
  }

  private BuildFormData(item: ItemCreateEdit): FormData {
    const formData = new FormData();
    formData.append('price', JSON.stringify(item.price));
    if (item.name){
    formData.append('name', item.name);
    }
    if (item.description){
    formData.append('description', item.description);
    }
    if (item.picture){
      formData.append('picture', item.picture);
    }
    if (item.categoriesIds) {
      formData.append('categoriesIds', JSON.stringify(item.categoriesIds));
    }
    if (item.discountsIds) {
      formData.append('discountsIds', JSON.stringify(item.discountsIds));
    }
    if (item.manufacturersIds) {
      formData.append('manufacturersIds', JSON.stringify(item.manufacturersIds));
    }
    if (item.tagsIds) {
      formData.append('tagsIds', JSON.stringify(item.tagsIds));
    }
    return formData;
  }

  private BuildFormData1(item: ItemEdit): FormData {
    const formData = new FormData();
      formData.append('id', JSON.stringify(item.id));
    
    formData.append('price', JSON.stringify(item.price));
    if (item.name){
    formData.append('name', item.name);
    }
    if (item.description){
    formData.append('description', item.description);
    }
    if (item.picture){
      formData.append('picture', item.picture);
    }
    if (item.categoriesIds) {
      formData.append('categoriesIds', JSON.stringify(item.categoriesIds));
    }
    if (item.discountsIds) {
      formData.append('discountsIds', JSON.stringify(item.discountsIds));
    }
    if (item.manufacturersIds) {
      formData.append('manufacturersIds', JSON.stringify(item.manufacturersIds));
    }
    if (item.tagsIds) {
      formData.append('tagsIds', JSON.stringify(item.tagsIds));
    }
    return formData;
  }

  // felipeov pokušaj
  createItem1(item: ItemCreateEdit) {
    const formData = this.BuildFormData2(item);
    return this.http.post(this.baseUrl + 'items', formData);
  }

  updateItem1(id: number, item: ItemCreateEdit){
    const formData = this.BuildFormData2(item);
    return this.http.put(this.baseUrl + 'items/' + id, formData);
  }

  private BuildFormData2(item: ItemCreateEdit): FormData {
    const formData = new FormData();
    /* if (item.id) {
      formData.append('id', JSON.stringify(item.id));
    } */

    formData.append('price', JSON.stringify(item.price));
    if (item.name){
    formData.append('name', item.name);
    }
    if (item.description){
    formData.append('description', item.description);
    }
    if (item.picture){
      formData.append('picture', item.picture);
    }
    return formData;
  }
}









