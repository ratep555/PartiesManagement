import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Item, ItemCreateEdit } from '../shared/models/item';
import { MyParams } from '../shared/models/myparams';
import { IPaginationForItems, PaginationForItems } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ItemsService {
  baseUrl = environment.apiUrl;
  items: Item[] = [];
  pagination = new PaginationForItems();
  myParams = new MyParams();
  itemCache = new Map();

  constructor(private http: HttpClient) { }

  getItems(useCache: boolean) {
    if (useCache === false) {
      this.itemCache = new Map();
    }
    if (this.itemCache.size > 0 && useCache === true) {
      if (this.itemCache.has(Object.values(this.myParams).join('-'))) {
        this.pagination.data = this.itemCache.get(Object.values(this.myParams).join('-'));
        return of(this.pagination);
      }
    }
    let params = new HttpParams();
    if (this.myParams.query) {
      params = params.append('query', this.myParams.query);
    }
    params = params.append('page', this.myParams.page.toString());
    params = params.append('pageCount', this.myParams.pageCount.toString());
    return this.http.get<IPaginationForItems>(this.baseUrl + 'items', { observe: 'response', params })
      .pipe(
        map(response => {
          this.itemCache.set(Object.values(this.myParams).join('-'), response.body.data);
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }

  createItem(item: ItemCreateEdit) {
    const formData = this.BuildFormData(item);
    return this.http.post(this.baseUrl + 'items', formData);
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

    return formData;
  }
}









