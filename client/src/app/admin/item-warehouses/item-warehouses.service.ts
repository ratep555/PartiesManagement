import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Item } from 'src/app/shared/models/item';
import { ItemWarehouse } from 'src/app/shared/models/itemWarehouses';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForItemWarehouses } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { Warehouse } from 'src/app/shared/models/warehouse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ItemWarehousesService {
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

  getItemWarehouses(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForItemWarehouses>(this.baseUrl + 'items/itemwarehouse', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  createItemWarehouse(discount: ItemWarehouse) {
    return this.http.post(this.baseUrl + 'items/itemwarehousecreate', discount);
  }

  updateItemWarehouse(id: number, warehouseid: number, params: any) {
    return this.http.put(this.baseUrl + 'items/itemwarehouseedit/' + id + '/' + warehouseid , params);
  }

  getItems() {
    return this.http.get<Item[]>(this.baseUrl + 'items/itemwarehouses/items');
  }

  getWarehouses() {
    return this.http.get<Warehouse[]>(this.baseUrl + 'items/itemwarehouses/warehouses');
  }

  getItemWarehouseByItemIdAndWarehouseId(id: number, warehouseid: number) {
    return this.http.get(this.baseUrl + 'items/itemwarehouse/' + id + '/' + warehouseid);
  }

}












